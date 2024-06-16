namespace MongoDDD.Tests.SUT.Contract.Read;

public record GetProducts(float MinimumScore)
{
    public record Result(IEnumerable<ProductDto> Products);
}
