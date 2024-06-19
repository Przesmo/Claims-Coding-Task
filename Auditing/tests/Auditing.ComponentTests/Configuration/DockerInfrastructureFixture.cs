using Testcontainers.RabbitMq;

namespace Auditing.ComponentTests.Configuration;

internal class DockerInfrastructureFixture : IAsyncDisposable
{
    private readonly RabbitMqContainer _rabbitMqContainer;

    public DockerInfrastructureFixture()
    {
        //ToDo: read data from settings
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3-management")
            .WithHostname("localhost")
            .WithPortBinding(15672, 15672)
            .WithPortBinding(5672, 5672)
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();
        _rabbitMqContainer.StartAsync().Wait();
    }

    public async ValueTask DisposeAsync()
    {
        await _rabbitMqContainer.DisposeAsync();
    }
}
