using Auditing.Infrastructure;
using Insurance.Application.DTOs;
using Insurance.Application.Messages.Queries;
using Insurance.Application.Services;
using Insurance.Infrastructure.Repositories.Covers;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Host.Controllers;

[ApiController]
//ToDo: Add versioning
[Route("[controller]")]
//ToDo: Add simple architecture tests
public class CoversController : ControllerBase
{
    private readonly ICoversService _coversService;
    private readonly ICoversRepository _coversRepository;
    private readonly Auditer _auditer;
    private readonly ILogger<CoversController> _logger;

    public CoversController(
        ICoversService coversService,
        ICoversRepository coversRepository,
        AuditContext auditContext,
        ILogger<CoversController> logger)
    {
        _coversService = coversService;
        _coversRepository = coversRepository;
        _auditer = new Auditer(auditContext);
        _logger = logger;
    }

    [HttpGet("premium/compute")]
    public decimal ComputePremiumAsync([FromQuery] ComputePremium query) =>
        _coversService.ComputePremium(query);

    [HttpGet]
    public async Task<IEnumerable<CoverDTO>> GetAllAsync([FromQuery] GetCovers query) =>
        await _coversService.GetAllAsync(query);

    [HttpGet("{id}")]
    //To Do: check if it will return 404 when resource is not found
    public async Task<CoverDTO> GetAsync([FromRoute] GetCover query)
    {
        return await _coversService.GetAsync(query);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        var computePremiumQuery = new ComputePremium
        {
            CoverType = cover.Type,
            StartDate = cover.StartDate,
            EndDate = cover.EndDate
        };
        cover.Premium = _coversService.ComputePremium(computePremiumQuery);
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
}
