namespace MongoDDD.Tests.SUT.Contract.Delete;

public record DeleteAndRetrieveProduct(string Id)
{
    public record Result(ProductDto Product);
}
