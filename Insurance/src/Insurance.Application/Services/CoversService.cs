using Insurance.Application.DTOs;
using Insurance.Application.Exceptions;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.AuditingIntegration;
using Insurance.Infrastructure.Repositories.Covers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    //ToDo: Fix and add tests
    public decimal ComputePremium(ComputePremium query)
    {
        var multiplier = 1.3m;
        if (query.CoverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (query.CoverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (query.CoverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = (query.EndDate - query.StartDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && query.CoverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && query.CoverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }

    public decimal ComputePremium2(ComputePremium query)
    {
        const int firstPeriodDays = 30;
        const int secondPeriodDays = 180;
        const decimal yachtDiscount1 = 0.05m;
        const decimal yachtDiscount2 = 0.08m;
        const decimal otherDiscount1 = 0.02m;
        const decimal otherDiscount2 = 0.03m;

        var multiplier = query.CoverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = (query.EndDate - query.StartDate).Days;

        var firstPeriod = Math.Min(insuranceLength, firstPeriodDays);
        var secondPeriod = Math.Min(Math.Max(insuranceLength - firstPeriod, 0), secondPeriodDays);
        var thirdPeriod = Math.Max(insuranceLength - secondPeriod, 0);

        var totalPremium = firstPeriod * premiumPerDay;

        if (query.CoverType == CoverType.Yacht)
        {
            totalPremium += secondPeriod * premiumPerDay * (1 - yachtDiscount1);
            totalPremium += thirdPeriod * premiumPerDay * (1 - yachtDiscount2);
        }
        else
        {
            totalPremium += secondPeriod * premiumPerDay * (1 - otherDiscount1);
            totalPremium += thirdPeriod * premiumPerDay * (1 - otherDiscount2);
        }

        return totalPremium;
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
            Premium = ComputePremium(computePremiumQuery),
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
