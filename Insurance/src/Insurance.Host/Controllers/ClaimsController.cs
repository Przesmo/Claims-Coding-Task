using Asp.Versioning;
using Insurance.Application.DTOs;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    public ClaimsController(IClaimsService claimsService)
    {
        _claimsService = claimsService;
    }

    [HttpGet]
    public async Task<IEnumerable<ClaimDTO>> GetAllAsync([FromQuery] GetClaims query) =>
        await _claimsService.GetAllAsync(query);

    [HttpPost]
    [Consumes("application/json")]
    public async Task<ClaimDTO> CreateAsync([FromBody] CreateClaim command) =>
        await _claimsService.CreateAsync(command);

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] DeleteClaim command) =>
        await _claimsService.DeleteAsync(command);

    [HttpGet("{id}")]
    public async Task<ClaimDTO?> GetAsync([FromRoute] GetClaim query) =>
        await _claimsService.GetAsync(query);
}
