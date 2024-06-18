using Xunit;

namespace Insurance.ComponentTests.Configuration;

[CollectionDefinition(nameof(ComponentTestsCollection))]
public class ComponentTestsCollection : ICollectionFixture<ComponentTestsFixture>
{
}
