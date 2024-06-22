namespace Insurance.Host.HealthChecks;

public static class HealthChecksConfiguration
{
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddMongoDb(configuration["MongoDb:ConnectionString"]!)
            .AddRabbitMQ(rabbitConnectionString: configuration["Bus:ConnectionString"]!);
        return services;
    }
}
