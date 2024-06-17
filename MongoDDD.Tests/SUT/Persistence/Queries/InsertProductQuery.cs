using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Create;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class InsertProductQuery(MongoClient client, ProductsDatabaseSettings settings)
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IWriteQuery<CreateProduct>
{
    public Task Write(CreateProduct queryData, CancellationToken token) => InsertOne(
        queryData.Id, 
        new(queryData.Name, queryData.Price), 
        new(0), 
        token);
}
