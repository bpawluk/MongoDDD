using MongoDB.Driver;
using MongoDDD.Core;
using MongoDDD.Core.Exceptions;
using MongoDDD.Persistence.Configuration;
using MongoDDD.Persistence.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Persistence
{
    public abstract class Repository<TAggregate, TAggregateState, TAggregateData> : Repository<TAggregate, TAggregateState, TAggregateData, None>
        where TAggregate : Aggregate<TAggregateState>, new()
    {
        protected Repository(MongoClient client, DatabaseSettings settings) : base(client, settings) { }

        protected abstract TAggregateState Map(TAggregateData aggregateData);

        protected override TAggregateState Map(TAggregateData aggregateData, None externalData) => Map(aggregateData);

        protected override None Initialize(TAggregateState aggregateState) => None.Value;
    }

    public abstract partial class Repository<TAggregate, TAggregateState, TAggregateData, TExternalData> : IRepository<TAggregate, TAggregateState>
        where TAggregate : Aggregate<TAggregateState>, new()
    {
        private readonly IMongoCollection<DatabaseDocument<TAggregateData, TExternalData>> _collection;

        public Repository(MongoClient client, DatabaseSettings settings)
        {
            _collection = client
                .GetDatabase(settings.Database)
                .GetCollection<DatabaseDocument<TAggregateData, TExternalData>>(settings.Collection);
        }

        protected abstract TAggregateState Map(TAggregateData aggregateData, TExternalData externalData);

        protected abstract TAggregateData Map(TAggregateState aggregateState);

        protected abstract TExternalData Initialize(TAggregateState aggregateState);

        public async Task Add(TAggregate aggregate, CancellationToken token)
        {
            if (aggregate is null)
            {
                throw new ArgumentException($"You cannot add an empty {typeof(TAggregate).Name}.");
            }

            var aggregateSnapshot = aggregate.CreateSnapshot();
            var newDocument = new DatabaseDocument<TAggregateData, TExternalData>(
                aggregateSnapshot.Id,
                Map(aggregateSnapshot.State),
                Initialize(aggregateSnapshot.State));

            try
            {
                await _collection.InsertOneAsync(newDocument, cancellationToken: token);
            }
            catch (MongoWriteException mongoWriteException)
            {
                if (mongoWriteException.WriteError.Category is ServerErrorCategory.DuplicateKey)
                {
                    throw new AlreadyExistsException($"{typeof(TAggregate).Name} with ID {newDocument.Id} already exists.");
                }
                throw new OperationFailedException($"Failed to add a new {typeof(TAggregate).Name}", mongoWriteException);
            }

            var newAggregateState = Map(newDocument.Data, newDocument.ExternalData);
            aggregate.Restore(new AggregateSnapshot<TAggregateState>(newDocument.Id, newDocument.Version, newAggregateState));
        }

        public async Task<TAggregate> Get(string id, CancellationToken token)
        {
            var document = await GetDocument(id, token);
            var aggregate = new TAggregate();
            var aggregateState = Map(document.Data, document.ExternalData);
            aggregate.Restore(new AggregateSnapshot<TAggregateState>(document.Id, document.Version, aggregateState));
            return aggregate;
        }

        public async Task Save(TAggregate aggregate, CancellationToken token)
        {
            if (aggregate is null)
            {
                throw new ArgumentException($"You cannot save an empty {typeof(TAggregate).Name}.");
            }

            var aggregateSnapshot = aggregate.CreateSnapshot();
            var filter = WithId(aggregateSnapshot.Id) & WithVersion(aggregateSnapshot.Version) & NotDeleted;
            var update = Update
                .Set(document => document.Data, Map(aggregateSnapshot.State))
                .Set(document => document.Modified, DateTime.UtcNow)
                .Inc(document => document.Version, 1);
            var options = new FindOneAndUpdateOptions<DatabaseDocument<TAggregateData, TExternalData>>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false
            };

            var updatedDocument = await _collection.FindOneAndUpdateAsync(filter, update, options, token);

            if (updatedDocument is null)
            {
                var document = await GetDocument(aggregateSnapshot.Id, token);
                if (document.Version != aggregateSnapshot.Version)
                {
                    throw new ConcurrentUpdateException(typeof(TAggregate).Name, aggregateSnapshot.Version, document.Version);
                }
                else
                {
                    throw new OperationFailedException($"Failed to save an existing {typeof(TAggregate).Name}");
                }
            }

            var newAggregateState = Map(updatedDocument.Data, updatedDocument.ExternalData);
            aggregate.Restore(new AggregateSnapshot<TAggregateState>(updatedDocument.Id, updatedDocument.Version, newAggregateState));
        }

        public async Task Remove(string id, CancellationToken token)
        {
            var filter = WithId(id) & NotDeleted;
            var update = Update.Set(document => document.Deleted, DateTime.UtcNow);
            await _collection.UpdateOneAsync(filter, update, cancellationToken: token);
        }
    }
}
