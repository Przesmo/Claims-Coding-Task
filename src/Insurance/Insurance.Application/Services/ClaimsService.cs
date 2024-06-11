using Auditing.Infrastructure;
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
    private readonly IAuditer _auditer;

    public ClaimsService(
        IClaimsRepository claimsRepository,
        ICoversService coversService,
        IAuditer auditer)
    {
        _claimsRepository = claimsRepository;
        _coversService = coversService;
        _auditer = auditer;
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
        _auditer.AuditClaim(claim.Id, "POST");

        return new ClaimDTO(claim.Created, claim.Type, claim.DamageCost)
        {
            Id = claim.Id,
            Name = claim.Name,
            CoverId = claim.CoverId
        };
    }

    public async Task DeleteAsync(DeleteClaim command)
    {
        await _claimsRepository.DeleteAsync(command.Id);
        _auditer.AuditClaim(command.Id, "DELETE");
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

    //ToDo: Fix handling null
    //ToDo: Improve Mapping
    public async Task<ClaimDTO> GetAsync(GetClaim query)
    {
        var claim = await _claimsRepository.GetAsync(query.Id);
        return new ClaimDTO(claim.Created, claim.Type, claim.DamageCost)
        {
            Id = claim.Id,
            CoverId = claim.CoverId,
            Name = claim.Name,
        };
    }
}
