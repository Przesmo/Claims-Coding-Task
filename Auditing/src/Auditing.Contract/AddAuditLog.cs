namespace Auditing.Host.Contracts;

public class AddAuditLog
{
    public required string EntityType { get; set; }
    public required string EntityId { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required string EntityChange { get; set; }
}
