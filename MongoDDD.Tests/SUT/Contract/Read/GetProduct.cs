namespace MongoDDD.Tests.SUT.Contract.Read;

public record GetProduct(string Id)
{
    public record Result(ProductDto Product);
}
