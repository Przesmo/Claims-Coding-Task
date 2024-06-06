using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.DTOs;

public class ClaimDTO
{
    public string Id { get; set; } = null!;

    public string CoverId { get; set; } = null!;

    public DateTime Created { get; set; }

    public string Name { get; set; } = null!;

    //ToDo: Think aboout placement
    public ClaimType Type { get; set; }

    public decimal DamageCost { get; set; }
}
