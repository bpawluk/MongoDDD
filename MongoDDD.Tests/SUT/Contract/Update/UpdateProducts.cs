namespace MongoDDD.Tests.SUT.Contract.Update;

public record UpdateProducts(IEnumerable<UpdateProducts.ProductUpdate> Products)
{
    public record ProductUpdate(string Id, string Name, decimal Price);
}
