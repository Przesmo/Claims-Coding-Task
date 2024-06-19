using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.DTOs;

//ToDo: Think aboout placement ClaimType
public record ClaimDTO(string Id, string CoverId, string Name, DateTime Created, ClaimType Type, decimal DamageCost);
