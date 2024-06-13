namespace Auditing.Host.Contracts;

public class AddAuditLog
{
    public string EntityType { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
    public string EntityChange { get; set; } = null!;
}
