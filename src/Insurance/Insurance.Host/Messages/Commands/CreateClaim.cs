using Insurance.Infrastructure.Repositories.Claims;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Host.Messages.Commands;

public class CreateClaim
{
    [Required]
    public string CoverId { get; set; } = null!;

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public ClaimType Type { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal DamageCost { get; set; }
}
