using Auditing.Host.Contracts;
using EasyNetQ;
using EasyNetQ.Topology;

namespace Insurance.Infrastructure.AuditingIntegration;

internal class AuditingQueue : IAuditingQueue
{
    private readonly IAdvancedBus _advancedBus;
    private readonly Exchange _exchange;
    private const string Key = "auditing";

    public AuditingQueue(IBus bus)
    {
        _advancedBus = bus.Advanced;
        _exchange = _advancedBus.ExchangeDeclare(typeof(AddAuditLog).ToString(), "direct");
    }

    public async Task PublishAsync(AddAuditLog message)
    {
        await _advancedBus.PublishAsync(_exchange, Key, false, new Message<AddAuditLog>(message));
    }
}
