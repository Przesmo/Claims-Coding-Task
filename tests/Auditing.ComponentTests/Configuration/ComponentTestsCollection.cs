using Xunit;

namespace Auditing.ComponentTests.Configuration;

[CollectionDefinition(nameof(ComponentTestsCollection))]
public class ComponentTestsCollection : ICollectionFixture<ComponentTestsFixture>
{
}
