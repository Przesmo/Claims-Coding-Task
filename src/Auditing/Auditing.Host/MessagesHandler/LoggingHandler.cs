using Auditing.Host.Contracts;
using EasyNetQ;
using Newtonsoft.Json;

namespace Auditing.Host.MessagesHandler;

public class LoggingHandler : IAddAuditLogHandler
{
    private readonly IAddAuditLogHandler _baseHandler;
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(IAddAuditLogHandler baseHandler, ILogger<LoggingHandler> logger)
    {
        _baseHandler = baseHandler;
        _logger = logger;
    }

    public async Task HandleAsync(
        AddAuditLog messageBody,
        MessageProperties messageProperties,
        CancellationToken cancellationToken = default)
    {
        var serializedMessage = JsonConvert.SerializeObject(messageBody);
        _logger.LogInformation("Starting handling {@receivedMessage}", serializedMessage);

        try
        {
            await _baseHandler.HandleAsync(messageBody, messageProperties, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to handle {@receivedMessage}", serializedMessage);
            throw;
        }
        finally
        {
            _logger.LogInformation("Finished handling {@receivedMessage}", serializedMessage);
        }
    }
}
