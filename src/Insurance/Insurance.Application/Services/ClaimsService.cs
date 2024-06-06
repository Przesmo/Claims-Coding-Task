using Insurance.Application.DTOs;
using Insurance.Host.Messages.Commands;
using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.Services;

public class ClaimsService : IClaimsService
{
    private readonly IClaimsRepository _claimsRepository;

    public ClaimsService(IClaimsRepository claimsRepository)
    {
        _claimsRepository = claimsRepository;
    }

    public async Task<ClaimDTO> CreateAsync(CreateClaim command)
    {
        var claim = new Claim
        {
            Id = Guid.NewGuid().ToString(),
            Type = command.Type,
            Name = command.Name,
            CoverId = command.CoverId,
            Created = command.Created,
            DamageCost = command.DamageCost
        };

        await _claimsRepository.CreateAsync(claim);

        return new ClaimDTO
        {
            Id = claim.Id,
            DamageCost = claim.DamageCost,
            Type = claim.Type,
            Name = claim.Name,
            CoverId = claim.CoverId,
            Created = claim.Created
        };
    }
}
