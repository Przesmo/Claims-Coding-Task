using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Host.Messages.Commands;
using Insurance.Host.Messages.Queries;
using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.Services;

//ToDo: Add architecture tests
public class ClaimsService : IClaimsService
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly ICoversService _coversService;

    public ClaimsService(IClaimsRepository claimsRepository, ICoversService coversService)
    {
        _claimsRepository = claimsRepository;
        _coversService = coversService;
    }

    public async Task<ClaimDTO> CreateAsync(CreateClaim command)
    {
        var isCovered = await _coversService.IsDateCoveredAsync(command.CoverId, command.Created);
        if (!isCovered)
        {
            throw new ClaimNotCoveredException(command.CoverId, command.Created);
        }

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

        return new ClaimDTO(claim.Created, claim.Type, claim.DamageCost)
        {
            Id = claim.Id,
            Name = claim.Name,
            CoverId = claim.CoverId
        };
    }

    public async Task<IEnumerable<ClaimDTO>> GetAllAsync(GetClaims query) =>
        (await _claimsRepository.GetAllAsync(query.Offset, query.Limit))
            //ToDo: It doesn't look nice having constructor and setting props
            .Select(x => new ClaimDTO(x.Created, x.Type, x.DamageCost)
            {
                CoverId = x.CoverId,
                Id = x.Id,
                Name = x.Name,
            });
}
