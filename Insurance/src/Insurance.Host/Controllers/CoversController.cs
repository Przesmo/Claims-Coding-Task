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
//ToDo: Add simple architecture tests
public class CoversController : ControllerBase
{
    private readonly ICoversService _coversService;
    private readonly ILogger<CoversController> _logger;

    public CoversController(
        ICoversService coversService,
        ILogger<CoversController> logger)
    {
        _coversService = coversService;
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
    public async Task<ActionResult> CreateAsync([FromBody] CreateCover command)
    {
        var cover = await _coversService.CreateAsync(command);
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] DeleteCover command)
    {
        await _coversService.DeleteAsync(command);
    }
}
