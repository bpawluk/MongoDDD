using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Update;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class UpdatePricesQuery(MongoClient client, ProductsDatabaseSettings settings)
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IWriteQuery<UpdatePrices>
{
    public async Task Write(UpdatePrices queryData, CancellationToken token)
    {
        var filter = Filter.In(document => document.Id, queryData.Ids);
        var update = Update.Set(document => document.Data.Price, queryData.NewPrice);
        await UpdateMany(filter, update, token);
    }
}
