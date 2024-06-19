using Asp.Versioning;
using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;

    private readonly ILogger<ClaimsController> _logger;

    public ClaimsController(
        IClaimsService claimsService,
        ILogger<ClaimsController> logger
        )
    {
        _claimsService = claimsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ClaimDTO>> GetAllAsync([FromQuery] GetClaims query) =>
        await _claimsService.GetAllAsync(query);

    [HttpPost]
    //To Do: Standarize when Action result when Task are returned etc.
    public async Task<ClaimDTO> CreateAsync([FromBody] CreateClaim command)
    {
        return await _claimsService.CreateAsync(command);
        //It will be moved to handler
        /*try
        {
        }
        //ToDo: Move it error handler
        catch (ClaimNotCoveredException ex)
        {
            _logger.LogError(ex, $"Claim cannot be created for cover: {command.CoverId}");
           // return BadRequest("Claim created date is not covered");
        }*/
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] DeleteClaim command)
    {
        await _claimsService.DeleteAsync(command);
    }

    [HttpGet("{id}")]
    public async Task<ClaimDTO?> GetAsync([FromRoute] GetClaim query)
    {
        return await _claimsService.GetAsync(query);
    }
}
