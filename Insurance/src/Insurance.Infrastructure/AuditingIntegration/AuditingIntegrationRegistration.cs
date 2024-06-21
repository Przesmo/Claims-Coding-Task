using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Infrastructure.AuditingIntegration;

public static class AuditingIntegrationRegistration
{
    public static IServiceCollection RegisterAuditingIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.RegisterEasyNetQ(configuration.GetSection("Bus:ConnectionString").Value)
            .AddScoped<IAuditingQueue, AuditingQueue>();
    }
}
