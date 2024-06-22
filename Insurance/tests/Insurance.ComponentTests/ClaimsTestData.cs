using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.ComponentTests;

public static class ClaimsTestData
{
    public static IEnumerable<Claim> Claims = new List<Claim>
    {
        new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow.AddDays(-10),
            DamageCost = 500,
            Name = "Claim1",
            Type = ClaimType.Collision
        },
        new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow.AddDays(-5),
            DamageCost = 20,
            Name = "Claim2",
            Type = ClaimType.BadWeather
        },
        new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow.AddDays(-10),
            DamageCost = 500,
            Name = "Claim3",
            Type = ClaimType.Grounding
        },
        new Claim
        {
            Id = Guid.NewGuid().ToString(),
            CoverId = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow.AddDays(-50),
            DamageCost = 10,
            Name = "Claim4",
            Type = ClaimType.Fire
        }
    };
}
