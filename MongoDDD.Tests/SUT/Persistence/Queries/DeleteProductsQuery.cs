using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Delete;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class DeleteProductsQuery(MongoClient client, ProductsDatabaseSettings settings)
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IWriteQuery<DeleteProducts>
{
    public async Task Write(DeleteProducts queryData, CancellationToken token)
    {
        var filter = Filter.In(document => document.Id, queryData.Ids);
        await DeleteMany(filter, token);
    }
}
