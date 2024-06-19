using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class GetCover
{
    [Required]
    public required string Id { get; set; }
}
