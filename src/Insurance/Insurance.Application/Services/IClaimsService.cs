using Insurance.Application.DTOs;
using Insurance.Host.Messages.Commands;
using Insurance.Host.Messages.Queries;

namespace Insurance.Application.Services;

public interface IClaimsService
{
    Task<ClaimDTO> CreateAsync(CreateClaim command);
    Task<IEnumerable<ClaimDTO>> GetAllAsync(GetClaims query);

}
