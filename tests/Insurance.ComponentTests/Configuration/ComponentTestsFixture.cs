using Insurance.Infrastructure.Repositories.Claims;
using Insurance.Infrastructure.Repositories.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Insurance.ComponentTests.Configuration;

public class ComponentTestsFixture : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _application = CustomWebApplicationFactory<Program>.Instance;
    private readonly DockerInfrastructureFixture _infrastructure = new();

    public HttpClient ApiHttpClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        ApiHttpClient = _application.CreateDefaultClient();

        await SeedDatabase();
    }

    public async Task DisposeAsync()
    {
        await _application.DisposeAsync();
        await _infrastructure.DisposeAsync();
    }

    private async Task SeedDatabase()
    {
        var mongoContext = _application.Services.GetRequiredService<InsuranceContext>();
        await mongoContext.Claims.AddAsync(new Claim
        {
            Created = DateTime.UtcNow,
            CoverId = "test",
            DamageCost = 1,
            Name = "test",
            Type = ClaimType.Fire
        });
        await mongoContext.SaveChangesAsync();
    }
}
