using Auditing.Host.Contracts;
using Auditing.Host.MessagesHandler;
using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;

namespace Auditing.Host.MessagesConsumer;

internal class CustomConsumer : IConsumer
{
    private readonly IBus _bus;
    private readonly IAddAuditLogHandler _messageHandler;
    private IDisposable? _queueConsumer;
    public Guid Id { get; } = Guid.NewGuid();

    public CustomConsumer(IBus bus, IAddAuditLogHandler messageHandler)
    {
        _bus = bus;
        _messageHandler = messageHandler;
    }

    public void StartConsuming()
    {
        //ToDo: Maybe think about some logging. In message handler as well
        _queueConsumer = _bus.Advanced.Consume<AddAuditLog>(DeclareQueue(),
            (message, _) => _messageHandler.HandleAsync(message.Body, message.Properties));
    }

    private Queue DeclareQueue()
    {
        var exchange = _bus.Advanced.ExchangeDeclare(typeof(AddAuditLog).ToString(), "direct");
        var queueName = "AuditingQueue";
        var queue = _bus.Advanced.QueueDeclare(queueName,
            options => options.AsDurable(true).AsAutoDelete(true).WithSingleActiveConsumer(true));
        _bus.Advanced.Bind(exchange, queue, "auditing");
        return queue;
    }

    public void Dispose()
    {
        _queueConsumer?.Dispose();
        _bus.Dispose();
    }
}
