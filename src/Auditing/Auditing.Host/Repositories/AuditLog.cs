namespace Auditing.Host.Repositories;

public class AuditLog
{
    public string EntityType { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
    public IDictionary<string, string> EntityMetadata { get; set; } = new Dictionary<string, string>();
}
