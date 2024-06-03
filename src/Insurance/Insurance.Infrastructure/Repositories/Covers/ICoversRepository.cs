using Claims;

namespace Insurance.Infrastructure.Repositories.Covers;

public interface ICoversRepository
{
    Task<IEnumerable<Cover>> GetAllAsync();
    Task<Cover> GetAsync(string id);
    Task CreateAsync(Cover cover);
    Task DeleteAsync(string id);
}
