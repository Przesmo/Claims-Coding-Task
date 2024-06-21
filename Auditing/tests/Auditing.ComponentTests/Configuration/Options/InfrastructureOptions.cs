using Microsoft.Extensions.Configuration;

namespace Auditing.ComponentTests.Configuration.Options;

internal class InfrastructureOptions
{
    public RabbitMQOptions RabbitMQOptions { get; }
    public MsSQLOptions MsSQLOptions { get; }

    public InfrastructureOptions()
    {
        var instance = ConfigurationBuilder()
            .Build();
        RabbitMQOptions = new RabbitMQOptions(instance["Bus:ConnectionString"]!);
        MsSQLOptions = new MsSQLOptions(instance["ConnectionStrings:DefaultConnection"]!);
    }

    private IConfigurationBuilder ConfigurationBuilder() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, false);
}
