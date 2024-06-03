﻿using Insurance.Infrastructure.Repositories.Database;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories.Covers;

internal class CoversRepository : ICoversRepository
{
    private readonly InsuranceContext _context;

    public CoversRepository(InsuranceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cover>> GetAllAsync()
    {
        return await _context.Covers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cover> GetAsync(string id)
    {
        return await _context.Covers
            .AsNoTracking()
            .Where(claim => claim.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task CreateAsync(Cover cover)
    {
        _context.Covers.Add(cover);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var claim = await GetAsync(id);
        if (claim is not null)
        {
            _context.Covers.Remove(claim);
            await _context.SaveChangesAsync();
        }
    }
}
