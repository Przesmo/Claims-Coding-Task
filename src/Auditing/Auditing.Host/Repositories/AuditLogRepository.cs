
using Auditing.Infrastructure;

namespace Auditing.Host.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AuditContext _context;

    public AuditLogRepository(AuditContext context)
    {
        _context = context;
    }

    public async Task AddAsync(AuditLog log, CancellationToken cancellationToken)
    {
        await _context.AddAsync(log, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
