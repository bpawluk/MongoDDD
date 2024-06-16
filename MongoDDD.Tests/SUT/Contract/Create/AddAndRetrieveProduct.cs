namespace MongoDDD.Tests.SUT.Contract.Create;

public record AddAndRetrieveProduct(string Id, string Name, decimal Price)
{
    public record Result(ProductDto Product);
}
