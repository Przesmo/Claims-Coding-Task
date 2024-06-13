using Auditing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Auditing.Host.Repositories;

public static class AuditLogsRepositoryRegistration
{
    public static IServiceCollection RegisterAuditLogsRepository(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        return services.AddDbContext<AuditContext>(options => options.UseSqlServer(connectionString),
            ServiceLifetime.Singleton)
            .AddSingleton<IAuditLogRepository, AuditLogRepository>();
    }
}
