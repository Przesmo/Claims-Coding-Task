using Auditing.Host.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace Auditing.ComponentTests.TestDoubles;

public class AuditLogRepositoryTestDouble : IAuditLogRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
    private const string AuditKey = "AuditLog";

    public AuditLogRepositoryTestDouble(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task AddAsync(AuditLog log, CancellationToken cancellationToken)
    {
        _memoryCache.Set(AuditKey, log, _cacheEntryOptions);
        return Task.CompletedTask;
    }

    public AuditLog? Get()
    {
        return _memoryCache.Get<AuditLog?>(AuditKey);
    }
}
