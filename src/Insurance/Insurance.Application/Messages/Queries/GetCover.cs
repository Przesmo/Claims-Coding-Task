using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class GetCover
{
    [Required]
    public string Id { get; set; } = null!;
}
