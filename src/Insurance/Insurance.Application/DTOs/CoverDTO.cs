using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.DTOs;

public record CoverDTO(string Id, DateTime StartDate, DateTime EndDate, CoverType Type, decimal Premium);
