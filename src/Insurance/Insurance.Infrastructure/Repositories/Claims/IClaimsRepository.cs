using Claims;

namespace Insurance.Infrastructure.Repositories.Claims;

public interface IClaimsRepository
{
    Task<IEnumerable<Claim>> GetAllAsync();
    Task<Claim> GetAsync(string id);
    Task AddAsync(Claim item);
    Task DeleteAsync(string id);
}
