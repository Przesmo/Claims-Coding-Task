using Xunit;

namespace Insurance.ComponentTests.Configuration;

internal class ComponentTestsFixture : IAsyncLifetime
{
    public Task DisposeAsync() => throw new NotImplementedException();
    public Task InitializeAsync() => throw new NotImplementedException();
}
