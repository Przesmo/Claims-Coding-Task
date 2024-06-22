using Insurance.Infrastructure.Repositories.Covers;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Commands;

public class CreateCover
{
    [Required]
    [DateIsGreaterThanToday]
    public DateTime StartDate { get; set; }

    [Required]
    [DateGreaterThan("StartDate")]
    public DateTime EndDate { get; set; }

    [Required]
    public CoverType Type { get; set; }
}
