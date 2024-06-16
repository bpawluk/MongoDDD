using MongoDB.Driver;
using MongoDDD.Core.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDDD.Persistence.Queries
{
    public partial class DatabaseQuery<TData, TExternalData>
    {
        protected async Task<DatabaseDocument<TData, TExternalData>> FindOneAndDelete(
            FilterDefinition<DatabaseDocument<TData, TExternalData>> filter,
            CancellationToken token)
        {
            var update = Update.Set(document => document.Deleted, DateTime.UtcNow);
            var options = new FindOneAndUpdateOptions<DatabaseDocument<TData, TExternalData>>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false
            };
            var document = await _collection.FindOneAndUpdateAsync(filter, update, options, token);

            if (document is null)
            {
                throw new DoesNotExistException($"{typeof(TData).Name} that matches the requested query does not exist.");
            }

            return document;
        }

        public async Task DeleteOne(FilterDefinition<DatabaseDocument<TData, TExternalData>> filter, CancellationToken token)
        {
            filter = filter & NotDeleted;
            var update = Update.Set(document => document.Deleted, DateTime.UtcNow);
            await _collection.UpdateOneAsync(filter, update, cancellationToken: token);
        }

        public async Task DeleteMany(FilterDefinition<DatabaseDocument<TData, TExternalData>> filter, CancellationToken token)
        {
            filter = filter & NotDeleted;
            var update = Update.Set(document => document.Deleted, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update, cancellationToken: token);
        }
    }
}
