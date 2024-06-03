using Claims.Auditing;
using Insurance.Infrastructure.Repositories.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly ILogger<ClaimsController> _logger;
    private readonly IClaimsRepository _claimsRepository;
    private readonly Auditer _auditer;

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
    public async Task<IEnumerable<Claim>> GetAsync()
    {
        return await _claimsRepository.GetAsync();
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        claim.Id = Guid.NewGuid().ToString();
        await _claimsRepository.AddAsync(claim);
        _auditer.AuditClaim(claim.Id, "POST");
        return Ok(claim);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        _auditer.AuditClaim(id, "DELETE");
        await _claimsRepository.DeleteAsync(id);
    }

    [HttpGet("{id}")]
    //To Do: check if it will return 404 when resource is not found
    public async Task<Claim> GetAsync(string id)
    {
        return await _claimsRepository.GetAsync(id);
    }
}