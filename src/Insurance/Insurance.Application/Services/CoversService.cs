using Insurance.Application.DTOs;
using Insurance.Application.Messages.Queries;
using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.Services;

public class CoversService : ICoversService
{
    private readonly ICoversRepository _coversRepository;

    public CoversService(ICoversRepository coversRepository)
    {
        _coversRepository = coversRepository;
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

    public async Task<IEnumerable<CoverDTO>> GetAllAsync(GetCovers query) =>
        (await _coversRepository.GetAllAsync(query.Offset, query.Limit))
            .Select(x => new CoverDTO(x.Id, x.StartDate, x.EndDate, x.Type, x.Premium));
    public async Task<CoverDTO> GetAsync(GetCover query)
    {
        var cover = await _coversRepository.GetAsync(query.Id);
        return new CoverDTO(cover.Id, cover.StartDate, cover.EndDate, cover.Type, cover.Premium);
    }

    //ToDo add test
    public async Task<bool> IsDateCoveredAsync(string coverId, DateTime date)
    {
        var cover = await _coversRepository.GetAsync(coverId);

        if (cover == null)
        {
            //ToDo think about it what to do 
            // Maybe it is business decision
            return false;
        }

        return cover.StartDate <= date && cover.EndDate >= date;
    }
}
