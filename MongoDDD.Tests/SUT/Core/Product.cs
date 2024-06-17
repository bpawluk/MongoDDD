using MongoDDD.Core;

namespace MongoDDD.Tests.SUT.Core;

public class Product : AggregateRoot<Product.State>
{
    public Product() { }

    public Product(string id, string name, decimal price) : base(id)
    {
        _state = new State(name, price, 0);
    }

    public void Rename(string newName)
    {
        _state = _state with { Name = newName };
    }

    public void SetPrice(decimal price)
    {
        _state = _state with { Price = price };
    }

    public record State(string Name, decimal Price, float ReviewScore);
}
