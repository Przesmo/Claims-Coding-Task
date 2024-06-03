using Claims;
using Insurance.Infrastructure.Repositories.Database;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories.Claims;

internal class ClaimsRepository : IClaimsRepository
{
    private readonly InsuranceContext _context;

    public ClaimsRepository(InsuranceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Claim>> GetAllAsync()
    {
        return await _context.Claims
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Claim> GetAsync(string id)
    {
        return await _context.Claims
            .AsNoTracking()
            .Where(claim => claim.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task AddItemAsync(Claim item)
    {
        _context.Claims.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteItemAsync(string id)
    {
        var claim = await GetClaimAsync(id);
        if (claim is not null)
        {
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
        }
    }
}
