using Insurance.Application.DTOs;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;

namespace Insurance.Application.Services;

public interface IClaimsService
{
    Task<ClaimDTO> CreateAsync(CreateClaim command);
    Task<IEnumerable<ClaimDTO>> GetAllAsync(GetClaims query);
    Task DeleteAsync(DeleteClaim command);
    Task<ClaimDTO> GetAsync(GetClaim id);
}
