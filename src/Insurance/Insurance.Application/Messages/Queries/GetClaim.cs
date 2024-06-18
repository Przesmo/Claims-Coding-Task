using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class GetClaim
{
    [Required]
    public required string Id { get; set; }
}
