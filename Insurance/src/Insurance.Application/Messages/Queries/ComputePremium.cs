using Insurance.Infrastructure.Repositories.Covers;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Queries;

public class ComputePremium
{
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public CoverType CoverType { get; set; }
}
