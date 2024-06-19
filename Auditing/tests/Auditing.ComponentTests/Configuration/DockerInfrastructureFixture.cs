using Auditing.ComponentTests.Configuration.Options;
using Testcontainers.RabbitMq;

namespace Auditing.ComponentTests.Configuration;

internal class DockerInfrastructureFixture : IAsyncDisposable
{
    private readonly RabbitMqContainer _rabbitMqContainer;

    public DockerInfrastructureFixture()
    {
        var infrastructureOptions = new InfrastructureOptions();
        var rabbitMQOptions = infrastructureOptions.RabbitMQOptions;
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithHostname(rabbitMQOptions.Host)
            .WithPortBinding(15672, 15672)
            .WithPortBinding(5672, 5672)
            .WithUsername(rabbitMQOptions.Username)
            .WithPassword(rabbitMQOptions.Password)
            .Build();
        _rabbitMqContainer.StartAsync().Wait();
    }

    public async ValueTask DisposeAsync()
    {
        await _rabbitMqContainer.DisposeAsync();
    }
}
