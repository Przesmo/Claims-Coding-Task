using Auditing.Infrastructure;
using Insurance.Application.Exceptions;
using Insurance.Application.Services;
using Insurance.Host.Messages.Commands;
using Insurance.Host.Messages.Queries;
using Insurance.Infrastructure.Repositories.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsService _claimsService;
    private readonly IClaimsRepository _claimsRepository;
    private readonly Auditer _auditer;
    private readonly ILogger<ClaimsController> _logger;

    public ClaimsController(
        IClaimsService claimsService,
        IClaimsRepository claimsContext,
        AuditContext auditContext,
        ILogger<ClaimsController> logger
        )
    {
        _claimsService = claimsService;
        _claimsRepository = claimsContext;
        _auditer = new Auditer(auditContext);
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<Claim>> GetAllAsync([FromQuery] GetClaims query)
    {
        return await _claimsRepository.GetAllAsync(query.Offset, query.Limit);
    }

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
    public async Task DeleteAsync(string id)
    {
        await _claimsRepository.DeleteAsync(id);
        _auditer.AuditClaim(id, "DELETE");
    }

    [HttpGet("{id}")]
    //To Do: check if it will return 404 when resource is not found
    public async Task<Claim> GetAsync(string id)
    {
        return await _claimsRepository.GetAsync(id);
    }
}