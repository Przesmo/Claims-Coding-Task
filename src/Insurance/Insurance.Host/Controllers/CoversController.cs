using Auditing.Infrastructure;
using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.Repositories.Covers;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
//ToDo: Add versioning
[Route("[controller]")]
//ToDo: Add simple architecture tests
public class CoversController : ControllerBase
{
    private readonly ICoversRepository _coversRepository;
    private readonly Auditer _auditer;
    private readonly ILogger<CoversController> _logger;

    public CoversController(
        ICoversRepository coversRepository,
        AuditContext auditContext,
        ILogger<CoversController> logger)
    {
        _coversRepository = coversRepository;
        _auditer = new Auditer(auditContext);
        _logger = logger;
    }

    [HttpPost("compute")]
    //To Do: Why Post not get?
    public async Task<ActionResult> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return Ok(ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAllAsync([FromQuery] GetCovers query)
    {
        var results = await _coversRepository.GetAllAsync(query.Offset, query.Limit);
        return Ok(results);
    }

    [HttpGet("{id}")]
    //To Do: check if it will return 404 when resource is not found
    public async Task<Cover> GetAsync(string id)
    {
        return await _coversRepository.GetAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _coversRepository.CreateAsync(cover);
        _auditer.AuditCover(cover.Id, "POST");
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        await _coversRepository.DeleteAsync(id);
        _auditer.AuditClaim(id, "DELETE");
    }

    private decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = (endDate - startDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}
