namespace Insurance.Infrastructure.Repositories.Claims;

public interface IClaimsRepository
{
    Task<IEnumerable<Claim>> GetAllAsync(int offset, int limit);
    Task<Claim> GetAsync(string id);
    Task CreateAsync(Claim claim);
    Task DeleteAsync(string id);
}
