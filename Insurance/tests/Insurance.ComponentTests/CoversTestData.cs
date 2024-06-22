using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.ComponentTests;

public static class CoversTestData
{
    public static string CoverToDeleteId = Guid.NewGuid().ToString();

    public static IEnumerable<Cover> Covers = new List<Cover>
    {
        new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow.AddDays(-100),
            EndDate = DateTime.UtcNow.AddDays(10),
            Premium = 200,
            Type = CoverType.BulkCarrier
        },
        new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow.AddDays(2),
            Premium = 100,
            Type = CoverType.Yacht
        },
        new Cover
        {
            Id = Guid.NewGuid().ToString(),
            StartDate = DateTime.UtcNow.AddDays(-50),
            EndDate = DateTime.UtcNow.AddDays(-10),
            Premium = 500,
            Type = CoverType.Tanker
        },
        new Cover
        {
            Id = CoverToDeleteId,
            StartDate = DateTime.UtcNow.AddDays(2),
            EndDate = DateTime.UtcNow.AddDays(20),
            Premium = 60,
            Type = CoverType.ContainerShip
        }
    };
}
