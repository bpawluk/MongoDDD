using MongoDB.Driver;
using MongoDDD.Core.Queries;
using MongoDDD.Persistence.Queries;
using MongoDDD.Tests.SUT.Contract.Update;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence.Queries;

internal class UpdateReviewScoreQuery(MongoClient client, ProductsDatabaseSettings settings) 
    : DatabaseQuery<ProductData, ExternalData>(client, settings)
    , IWriteQuery<UpdateReviewScore>
{
    public async Task Write(UpdateReviewScore queryData, CancellationToken token)
    {
        var filter = WithId(queryData.ProductId);
        var update = SilentUpdate.Set(document => document.ExternalData.ReviewScore, queryData.NewScore);
        await UpdateOne(filter, update, token);
    }
}
