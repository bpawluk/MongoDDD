using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Delete;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class DeleteProductQuery(MongoClient client, ProductsDatabaseSettings settings)
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IWriteQuery<DeleteAndRetrieveProduct, DeleteAndRetrieveProduct.Result>
    , IWriteQuery<DeleteProduct>
{
    public async Task<DeleteAndRetrieveProduct.Result> Write(DeleteAndRetrieveProduct queryData, CancellationToken token)
    {
        var document = await FindOneAndDelete(WithId(queryData.Id), token);
        return new(new(document.Id, document.Data.Name, document.Data.Price, document.ExternalData.ReviewScore));
    }

    public Task Write(DeleteProduct queryData, CancellationToken token) => DeleteOne(WithId(queryData.Id), token);
}
