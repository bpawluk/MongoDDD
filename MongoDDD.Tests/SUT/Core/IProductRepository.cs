using MongoDDD.Core;

namespace MongoDDD.Tests.SUT.Core;

public interface IProductRepository : IRepository<Product, Product.State> { }
