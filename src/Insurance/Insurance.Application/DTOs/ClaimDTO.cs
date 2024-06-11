using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.DTOs;

//ToDo: Think aboout placement ClaimType
public record ClaimDTO(DateTime Created, ClaimType Type, decimal DamageCost)
{
    public string Id { get; set; } = null!;

    public string CoverId { get; set; } = null!;

    public string Name { get; set; } = null!;
}
