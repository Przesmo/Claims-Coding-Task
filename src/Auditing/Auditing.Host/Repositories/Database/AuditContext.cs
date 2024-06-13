using Auditing.Host.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auditing.Infrastructure;

public class AuditContext : DbContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }
    public DbSet<AuditLog> AuditLogs { get; set; }
}
