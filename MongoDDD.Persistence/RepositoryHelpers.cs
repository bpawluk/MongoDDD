using MongoDB.Driver;
using MongoDDD.Core;
using MongoDDD.Core.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Persistence
{
    public abstract partial class Repository<TAggregate, TAggregateState, TAggregateData, TExternalData> : IRepository<TAggregate, TAggregateState>
        where TAggregate : Aggregate<TAggregateState>, new()
    {
        private static FilterDefinitionBuilder<DatabaseDocument<TAggregateData, TExternalData>> Filter =>
            Builders<DatabaseDocument<TAggregateData, TExternalData>>.Filter;

        private static UpdateDefinitionBuilder<DatabaseDocument<TAggregateData, TExternalData>> Update =>
            Builders<DatabaseDocument<TAggregateData, TExternalData>>.Update;

        private static FilterDefinition<DatabaseDocument<TAggregateData, TExternalData>> NotDeleted =>
            Filter.Eq(document => document.Deleted, null);

        private static FilterDefinition<DatabaseDocument<TAggregateData, TExternalData>> WithId(string id) =>
            Filter.Eq(document => document.Id, id);

        private static FilterDefinition<DatabaseDocument<TAggregateData, TExternalData>> WithVersion(int version) =>
            Filter.Eq(document => document.Version, version);

        private async Task<DatabaseDocument<TAggregateData, TExternalData>> GetDocument(string id, CancellationToken token)
        {
            var filter = WithId(id) & NotDeleted;
            var document = await _collection.Find(filter).Limit(1).SingleOrDefaultAsync(token);
            return document ?? throw new DoesNotExistException($"{typeof(TAggregate).Name} with ID {id} does not exist.");
        }
    }
}
