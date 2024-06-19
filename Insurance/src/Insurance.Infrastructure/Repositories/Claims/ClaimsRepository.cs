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

    public async Task<IEnumerable<Claim>> GetAllAsync(int offset, int limit) =>
        await _context.Claims
            .AsNoTracking()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

    public async Task<Claim?> GetAsync(string id) =>
        await _context.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(claim => claim.Id == id);

    public async Task CreateAsync(Claim claim)
    {
        _context.Claims.Add(claim);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var claim = await GetAsync(id);
        if (claim is not null)
        {
            _context.Claims.Remove(claim);
            await _context.SaveChangesAsync();
        }
    }
}
