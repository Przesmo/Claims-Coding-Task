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
public class CoversController : ControllerBase
{
    private readonly ICoversService _coversService;

    public CoversController(ICoversService coversService)
    {
        _coversService = coversService;
    }

    [HttpGet("premium")]
    public decimal ComputePremiumAsync([FromQuery] ComputePremium query) =>
       PremiumCalculator.Calculate(query);

    [HttpGet]
    public async Task<IEnumerable<CoverDTO>> GetAllAsync([FromQuery] GetCovers query) =>
        await _coversService.GetAllAsync(query);

    [HttpGet("{Id}")]
    public async Task<CoverDTO?> GetAsync([FromRoute] GetCover query) =>
        await _coversService.GetAsync(query);

    [HttpPost]
    [Consumes("application/json")]
    public async Task<CoverDTO> CreateAsync([FromBody] CreateCover command) =>
        await _coversService.CreateAsync(command);

    [HttpDelete("{Id}")]
    public async Task DeleteAsync([FromRoute] DeleteCover command) =>
        await _coversService.DeleteAsync(command);
}
