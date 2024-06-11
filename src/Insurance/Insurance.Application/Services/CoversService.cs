using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.Services;

public class CoversService : ICoversService
{
    private readonly ICoversRepository _coversRepository;

    public CoversService(ICoversRepository coversRepository)
    {
        _coversRepository = coversRepository;
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
