using Auditing.Host.Repositories;
using Auditing.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Auditing.ComponentTests.TestHelpers;

public class AuditTestRepository
{
    private readonly AuditContext _context;

    public AuditTestRepository(AuditContext context)
    {
        _context = context;
    }

    public async Task<AuditLog> GetFirstAsync() =>
        await _context.AuditLogs.AsNoTracking().FirstAsync();
}
