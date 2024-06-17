using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Read;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class FindProductQuery(MongoClient client, ProductsDatabaseSettings settings)
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IReadQuery<GetProduct, GetProduct.Result>
{
    public async Task<GetProduct.Result> Read(GetProduct queryData, CancellationToken token)
    {
        var filter = WithId(queryData.Id);
        var result = await FindOne(filter, token);
        return new(new(result.Id, result.Data.Name, result.Data.Price, result.ExternalData.ReviewScore));
    }
}
