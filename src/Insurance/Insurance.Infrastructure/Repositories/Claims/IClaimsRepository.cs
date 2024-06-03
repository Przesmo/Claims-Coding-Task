using Claims;

namespace Insurance.Infrastructure.Repositories.Claims;

public interface IClaimsRepository
{
    Task<IEnumerable<Claim>> GetAllAsync();
    Task<Claim> GetAsync(string id);
    Task CreateAsync(Claim claim);
    Task DeleteAsync(string id);
}
