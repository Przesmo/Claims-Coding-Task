using Auditing.Host.Contracts;
using Insurance.Infrastructure.AuditingIntegration;

namespace Insurance.ComponentTests.TestDoubles;

internal class AuditingQueueTestDouble : IAuditingQueue
{
    public Task PublishAsync(AddAuditLog message) => Task.CompletedTask;
}
