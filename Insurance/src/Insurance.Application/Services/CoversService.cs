using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.AuditingIntegration;
using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.Services;

public class CoversService : ICoversService
{
    private readonly ICoversRepository _coversRepository;
    private readonly IAuditingQueue _auditingQueue;

    public CoversService(ICoversRepository coversRepository, IAuditingQueue auditingQueue)
    {
        _coversRepository = coversRepository;
        _auditingQueue = auditingQueue;
    }

    public async Task<CoverDTO> CreateAsync(CreateCover command)
    {
        var insuranceLength = (command.EndDate - command.StartDate).Days;
        if (insuranceLength > CoversOptions.MaximumInsurancePeriodDays)
        {
            throw new InsurancePeriodExceededException(CoversOptions.MaximumInsurancePeriodDays);
        }

        var computePremiumQuery = new ComputePremium
        {
            CoverType = command.Type,
            StartDate = command.StartDate,
            EndDate = command.EndDate
        };
        var cover = new Cover
        {
            Id = Guid.NewGuid().ToString(),
            Premium = PremiumCalculator.Calculate(computePremiumQuery),
            EndDate = command.EndDate,
            StartDate = command.StartDate,
            Type = command.Type
        };
        await _coversRepository.CreateAsync(cover);
        await _auditingQueue.PublishAsync(new()
        {
            EntityChange = "Create",
            EntityId = cover.Id,
            EntityType = "Cover",
            TimeStamp = DateTime.UtcNow
        });

        return new CoverDTO(cover.Id, cover.StartDate, cover.EndDate, cover.Type, cover.Premium);
    }

    public async Task DeleteAsync(DeleteCover command)
    {
        await _coversRepository.DeleteAsync(command.Id);
        await _auditingQueue.PublishAsync(new()
        {
            EntityChange = "Delete",
            EntityId = command.Id,
            EntityType = "Cover",
            TimeStamp = DateTime.UtcNow
        });
    }

    public async Task<IEnumerable<CoverDTO>> GetAllAsync(GetCovers query) =>
        (await _coversRepository.GetAllAsync(query.Offset, query.Limit))
            .Select(x => new CoverDTO(x.Id, x.StartDate, x.EndDate, x.Type, x.Premium));

    public async Task<CoverDTO?> GetAsync(GetCover query)
    {
        var cover = await _coversRepository.GetAsync(query.Id);
        return cover is null ? null :
            new CoverDTO(cover.Id, cover.StartDate, cover.EndDate, cover.Type, cover.Premium);
    }

    public async Task<bool> IsDateCoveredAsync(IsDateCovered query)
    {
        var cover = await _coversRepository.GetAsync(query.CoverId);

        if (cover == null)
        {
            return false;
        }

        return cover.StartDate <= query.DateToCover && cover.EndDate >= query.DateToCover;
    }
}
