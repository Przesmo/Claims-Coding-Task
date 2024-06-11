using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.DTOs;

public record CoverDTO(DateTime StartDate, DateTime EndDate, CoverType Type, decimal Premium)
{
    public string Id { get; set; } = null!;
}
