using MongoDB.Driver;
using MongoDDD.Persistence;
using MongoDDD.Tests.SUT.Core;
using MongoDDD.Tests.SUT.Persistence.Configuration;
using MongoDDD.Tests.SUT.Persistence.Data;

namespace MongoDDD.Tests.SUT.Persistence;

internal class ProductRepository(MongoClient client, ProductsDatabaseSettings settings, IServiceProvider services) 
    : Repository<Product, Product.State, ProductData>(client, settings, services)
    , IProductRepository
{
    protected override Product.State Map(ProductData aggregateData) => new(aggregateData.Name, aggregateData.Price, 0);

    protected override ProductData Map(Product.State aggregateState) => new(aggregateState.Name, aggregateState.Price);
}

internal class ProductRepositoryWithExternalData(MongoClient client, ProductsDatabaseSettings settings, IServiceProvider services) 
    : Repository<Product, Product.State, ProductData, ExternalData>(client, settings, services)
    , IProductRepository
{
    protected override ExternalData Initialize(Product.State aggregateState) => new(0);

    protected override Product.State Map(ProductData aggregateData, ExternalData externalData) 
        => new(aggregateData.Name, aggregateData.Price, externalData.ReviewScore);

    protected override ProductData Map(Product.State aggregateState) 
        => new(aggregateState.Name, aggregateState.Price);
}