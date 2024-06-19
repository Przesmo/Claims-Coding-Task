using Insurance.Infrastructure.Repositories.Claims;
using Insurance.Infrastructure.Repositories.Covers;
using Insurance.Infrastructure.Repositories.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Insurance.Infrastructure.Repositories;

public static class RepositoriesRegistration
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<InsuranceContext>(options =>
            {
                var client = new MongoClient(configuration["MongoDb:ConnectionString"]!);
                var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]!);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            })
            .AddScoped<IClaimsRepository, ClaimsRepository>()
            .AddScoped<ICoversRepository, CoversRepository>();
    }
}
