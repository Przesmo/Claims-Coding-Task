using Auditing.Infrastructure;
using Insurance.Host.Messages.Queries;
using Insurance.Infrastructure.Repositories.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly Auditer _auditer;
    private readonly ILogger<ClaimsController> _logger;

    public ClaimsController(
        IClaimsRepository claimsContext,
        AuditContext auditContext,
        ILogger<ClaimsController> logger)
    {
        _claimsRepository = claimsContext;
        _auditer = new Auditer(auditContext);
        _logger = logger;
    }

    [HttpGet]
    //To Do: Do some pagination
    public async Task<IEnumerable<Claim>> GetAllAsync([FromQuery]GetClaims query)
    {
        return await _claimsRepository.GetAllAsync(query.Offset, query.Limit);
    }

    [HttpPost]
    //To Do: Standarize when Action result when Task are returned etc.
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        claim.Id = Guid.NewGuid().ToString();
        await _claimsRepository.CreateAsync(claim);
        _auditer.AuditClaim(claim.Id, "POST");
        return Ok(claim);
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