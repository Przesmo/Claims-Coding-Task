using Auditing.ComponentTests.Configuration.Options;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;

namespace Auditing.ComponentTests.Configuration;

internal class DockerInfrastructureFixture : IAsyncDisposable
{
    private readonly MsSqlContainer _msSqlContainer;
    private readonly RabbitMqContainer _rabbitMqContainer;

    public DockerInfrastructureFixture()
    {
        var infrastructureOptions = new InfrastructureOptions();

        var msSqlOptions = infrastructureOptions.MsSQLOptions;
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPortBinding(57809, 1433)
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("SQLCMDUSER", msSqlOptions.Username)
            .WithEnvironment("SQLCMDPASSWORD", msSqlOptions.Password)
            .WithEnvironment("MSSQL_SA_PASSWORD", msSqlOptions.Password)
            .Build();
        _msSqlContainer.StartAsync().Wait();

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
        await _msSqlContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
    }
}
