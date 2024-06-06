using Insurance.Application.DTOs;
using Insurance.Host.Messages.Commands;

namespace Insurance.Application.Services;

public interface IClaimsService
{
    Task<ClaimDTO> CreateAsync(CreateClaim command);
}
