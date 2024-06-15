using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class GetClaim
{
    [Required]
    public string Id { get; set; } = null!;
}
