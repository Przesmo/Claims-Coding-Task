using Microsoft.Extensions.Configuration;

namespace Insurance.ComponentTests.Configuration.Options;

internal class InfrastructureOptions
{
    public MongoDbOptions MongoDbOptions { get; }

    public InfrastructureOptions()
    {
        var instance = ConfigurationBuilder()
            .Build();
        MongoDbOptions = new MongoDbOptions(instance["MongoDb:ConnectionString"]!);
    }

    private IConfigurationBuilder ConfigurationBuilder() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, false);
}
