using Auditing.Infrastructure;
using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly Auditer _auditer;
    private readonly ILogger<ClaimsController> _logger;

    public ClaimsController(
        IClaimsService claimsService,
        AuditContext auditContext,
        ILogger<ClaimsController> logger
        )
    {
        _claimsService = claimsService;
        _auditer = new Auditer(auditContext);
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ClaimDTO>> GetAllAsync([FromQuery] GetClaims query) =>
        await _claimsService.GetAllAsync(query);

    [HttpPost]
    //To Do: Standarize when Action result when Task are returned etc.
    public async Task<ActionResult> CreateAsync([FromBody] CreateClaim command)
    {
        try
        {
            var claim = await _claimsService.CreateAsync(command);
            //ToDo: Move Audit to Application layer
            _auditer.AuditClaim(claim.Id, "POST");
            return Ok(claim);
        }
        //ToDo: Move it error handler
        catch (ClaimNotCoveredException ex)
        {
            _logger.LogError(ex, $"Claim cannot be created for cover: {command.CoverId}");
            return BadRequest("Claim created date is not covered");
        }
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] DeleteClaim command)
    {
        await _claimsService.DeleteAsync(command);
        _auditer.AuditClaim(command.Id, "DELETE");
    }

    [HttpGet("{id}")]
    //To Do: check if it will return 404 when resource is not found
    public async Task<ClaimDTO> GetAsync([FromRoute] GetClaim query)
    {
        return await _claimsService.GetAsync(query);
    }
}