namespace MongoDDD.Tests.SUT.Contract.Update;

public record UpdatePrices(IEnumerable<string> Ids, decimal NewPrice);
