using Insurance.Infrastructure.Repositories.Covers;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class ComputePremium
{
    [Required]
    [DateIsGreaterThanToday]
    public DateTime StartDate { get; set; }

    [Required]
    [DateGreaterThan("StartDate")]
    public DateTime EndDate { get; set; }

    [Required]
    public CoverType CoverType { get; set; }
}
