using Insurance.Infrastructure.Repositories.Claims;
using Insurance.Infrastructure.Repositories.Covers;
using Insurance.Infrastructure.Repositories.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Insurance.Infrastructure.Repositories;

public static class RepositoriesRegistration
{
    //To Do: instead of passsing values can inject config
    //To Do: double-check mongo registration
    public static IServiceCollection RegisterRepositories(this IServiceCollection services, string connectionString, string databaseName)
    {
        return services.AddDbContext<InsuranceContext>(options =>
            {
                var client = new MongoClient(connectionString);
                var database = client.GetDatabase(databaseName);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            })
            .AddScoped<IClaimsRepository, ClaimsRepository>()
            .AddScoped<ICoversRepository, CoversRepository>();
    }
}
