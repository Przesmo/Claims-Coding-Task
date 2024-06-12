﻿using Auditing.Host.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auditing.Infrastructure;

public class AuditContext : DbContext
{
    public AuditContext(DbContextOptions<AuditContext> options) : base(options)
    {
    }
    public DbSet<ClaimAudit> ClaimAudits { get; set; }
    public DbSet<CoverAudit> CoverAudits { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}
