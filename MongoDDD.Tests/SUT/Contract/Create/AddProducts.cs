namespace MongoDDD.Tests.SUT.Contract.Create;

public record AddProducts(IEnumerable<AddProducts.NewProduct> Products)
{
    public record NewProduct(string Id, string Name, decimal Price);
}
