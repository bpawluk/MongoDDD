namespace MongoDDD.Tests;

[CollectionDefinition(nameof(TestsCollection))]
public class TestsCollection : ICollectionFixture<TestsFixture> { }
