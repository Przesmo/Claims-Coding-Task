using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.Repositories.Claims;

namespace Insurance.Application.Services;

public class ClaimsService : IClaimsService
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly ICoversService _coversService;
    //private readonly IAuditer _auditer;

    public ClaimsService(
        IClaimsRepository claimsRepository,
        ICoversService coversService)
    //IAuditer auditer)
    {
        _claimsRepository = claimsRepository;
        _coversService = coversService;
        //_auditer = auditer;
    }

    public async Task<ClaimDTO> CreateAsync(CreateClaim command)
    {
        var isCovered = await _coversService.IsDateCoveredAsync(
            new IsDateCovered{ CoverId = command.CoverId, DateToCover = command.Created });
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
        //_auditer.AuditClaim(claim.Id, "POST");

        return new ClaimDTO(claim.Id, claim.CoverId, claim.Name, claim.Created, claim.Type, claim.DamageCost);
    }

    public async Task DeleteAsync(DeleteClaim command)
    {
        await _claimsRepository.DeleteAsync(command.Id);
        //_auditer.AuditClaim(command.Id, "DELETE");
    }

    public async Task<IEnumerable<ClaimDTO>> GetAllAsync(GetClaims query) =>
        (await _claimsRepository.GetAllAsync(query.Offset, query.Limit))
            .Select(x => new ClaimDTO(x.Id, x.CoverId, x.Name, x.Created, x.Type, x.DamageCost));

    public async Task<ClaimDTO?> GetAsync(GetClaim query)
    {
        var claim = await _claimsRepository.GetAsync(query.Id);
        return claim is null ? null :
            new ClaimDTO(claim.Id, claim.CoverId, claim.Name, claim.Created, claim.Type, claim.DamageCost);
    }
}
