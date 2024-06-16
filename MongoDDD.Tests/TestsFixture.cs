using Mongo2Go;

namespace MongoDDD.Tests;

public sealed class TestsFixture : IDisposable
{
    public MongoDbRunner MongoRunner { get; }

    public TestsFixture()
    {
        MongoRunner = MongoDbRunner.Start();
    }

    public void Dispose() => MongoRunner.Dispose();
}
