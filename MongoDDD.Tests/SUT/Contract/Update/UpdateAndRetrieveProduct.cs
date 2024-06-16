namespace MongoDDD.Tests.SUT.Contract.Update;

public record UpdateAndRetrieveProduct(string Id, string Name, decimal Price)
{
    public record Result(ProductDto Product);
}
