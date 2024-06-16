namespace MongoDDD.Tests.SUT.Contract.Read;

public record GetProductsPage(int PageNumber, int PageSize)
{
    public record Result(int PageNumber, int NumberOfPages, IEnumerable<ProductDto> Products);
}
