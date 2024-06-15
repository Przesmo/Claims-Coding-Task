using Auditing.Host.Contracts;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Auditing.Messeging.Tests;

public class MessagingTests
{
    [Fact]
    public async Task PostMessage()
    {
        //ToDo: Refactor
        var connectionString = "host=localhost;username=guest;password=guest";
        var services = new ServiceCollection();
        services.RegisterEasyNetQ(connectionString);
        var bus = services.BuildServiceProvider().GetRequiredService<IBus>();
        var advancesBus = bus.Advanced;

        var exchange = advancesBus.ExchangeDeclare(typeof(AddAuditLog).ToString(), "direct");
        var key = "auditing";
        var message = new Message<AddAuditLog>(new AddAuditLog
        {
            EntityId = "123",
            EntityChange = "test",
            EntityType = "wer",
            TimeStamp = DateTime.UtcNow,
        });
        await advancesBus.PublishAsync(exchange, key, false, message);
 
    }
}