
using EasyNetQ.Consumer;

namespace Auditing.Host;

public class ConsumerSubscriptionService : IHostedService
{
    private readonly IConsumer _consumer;

    public ConsumerSubscriptionService(IConsumer consumer)
    {
        _consumer = consumer;
    }

    public Task StartAsync(CancellationToken cancellationToken) =>
        Task.Run(() => _consumer.StartConsuming(), cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) =>
        Task.Run(() => _consumer.Dispose(), cancellationToken);
}
