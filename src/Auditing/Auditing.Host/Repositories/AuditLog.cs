using System.ComponentModel.DataAnnotations;

namespace Auditing.Host.Repositories;

public class AuditLog
{
    [Key]
    public int Id { get; set; }
    public string EntityType { get; set; } = null!;
    public string EntityChange { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
}
