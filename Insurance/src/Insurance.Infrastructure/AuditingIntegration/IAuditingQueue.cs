namespace Insurance.Infrastructure.AuditingIntegration;

public interface IAuditingQueue
{
    Task PublishAsync(AddAuditLog message);
}
