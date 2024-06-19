using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Commands;

public class DeleteClaim
{
    [Required]
    public required string Id { get; set; }
}
