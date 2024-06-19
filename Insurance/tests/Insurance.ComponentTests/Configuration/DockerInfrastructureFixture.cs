using Insurance.ComponentTests.Configuration.Options;
using Testcontainers.MongoDb;

namespace Insurance.ComponentTests.Configuration;

internal class DockerInfrastructureFixture : IAsyncDisposable
{
    private readonly MongoDbContainer _mongoDbContainer;

    public DockerInfrastructureFixture()
    {
        var infrastructureOptions = new InfrastructureOptions();
        var mongoDbOptions = infrastructureOptions.MongoDbOptions;
        _mongoDbContainer = new MongoDbBuilder()
            .WithPortBinding(mongoDbOptions.Port, mongoDbOptions.Port)
            .WithUsername(mongoDbOptions.Username)
            .WithPassword(mongoDbOptions.Password)
            .Build();
        _mongoDbContainer.StartAsync().Wait();
    }

    public async ValueTask DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}
