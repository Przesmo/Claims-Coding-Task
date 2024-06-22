using Auditing.Host.Contracts;
using EasyNetQ;

namespace Insurance.Infrastructure.AuditingIntegration;

internal class AuditingQueue : IAuditingQueue
{
    private readonly IAdvancedBus _advancedBus;
    private const string Key = "auditing";

    public AuditingQueue(IBus bus)
    {
        _advancedBus = bus.Advanced;
    }

    public async Task PublishAsync(AddAuditLog message)
    {
        var exchange = _advancedBus.ExchangeDeclare(typeof(AddAuditLog).ToString(), "direct");
        await _advancedBus.PublishAsync(exchange, Key, false, new Message<AddAuditLog>(message));
    }
}
