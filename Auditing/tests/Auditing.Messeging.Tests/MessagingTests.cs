using Auditing.Host.Contracts;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Auditing.Messeging.Tests;

public class MessagingTests
{
    [Fact(Skip = "Only for quick manual tests")]
    public async Task PostMessage()
    {
        var advancedBus = CreateAdvancedBus();
        var exchange = advancedBus.ExchangeDeclare(typeof(AddAuditLog).ToString(), "direct");
        var key = "auditing";
        var message = new Message<AddAuditLog>(new AddAuditLog
        {
            EntityId = "123",
            EntityChange = "test",
            EntityType = "wer",
            TimeStamp = DateTime.UtcNow,
        });

        await advancedBus.PublishAsync(exchange, key, false, message);
    }

    private IAdvancedBus CreateAdvancedBus()
    {
        var connectionString = "host=localhost;username=guest;password=guest";
        var services = new ServiceCollection();
        services.RegisterEasyNetQ(connectionString);
        var bus = services.BuildServiceProvider().GetRequiredService<IBus>();
        return bus.Advanced;
    }
}
