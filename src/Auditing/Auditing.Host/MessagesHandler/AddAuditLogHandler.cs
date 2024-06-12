using Auditing.Host.Contracts;
using EasyNetQ;

namespace Auditing.Host.MessagesHandler
{
    public class AddAuditLogHandler : IAddAuditLogHandler
    {
        public Task HandleAsync(AddAuditLog messageBody, MessageProperties messageProperties, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
