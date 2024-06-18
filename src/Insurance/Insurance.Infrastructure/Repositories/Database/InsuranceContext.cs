﻿using Insurance.Infrastructure.Repositories.Claims;
using Insurance.Infrastructure.Repositories.Covers;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Insurance.Infrastructure.Repositories.Database;

public class InsuranceContext : DbContext
{
    public DbSet<Claim> Claims { get; init; }
    public DbSet<Cover> Covers { get; init; }

    public InsuranceContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Claim>().ToCollection("claims");
        modelBuilder.Entity<Cover>().ToCollection("covers");
    }
}
