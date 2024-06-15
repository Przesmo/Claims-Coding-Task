using Auditing.Host.Contracts;
using EasyNetQ;

namespace Auditing.Host.MessagesHandler;

public interface IAddAuditLogHandler
{
    Task HandleAsync(AddAuditLog messageBody, MessageProperties messageProperties,
        CancellationToken cancellationToken = default);
}
