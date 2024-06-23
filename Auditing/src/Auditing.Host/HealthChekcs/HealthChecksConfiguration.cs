using Microsoft.Extensions.Configuration;

namespace Auditing.Host.HealthChekcs;

public static class HealthChecksConfiguration
{
    public static IServiceCollection ConfigureHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);
        return services;
    }
}
