﻿using Insurance.Infrastructure.Repositories.Claims;
using System.ComponentModel.DataAnnotations;

namespace Insurance.Application.Messages.Commands;

public class CreateClaim
{
    [Required]
    public required string CoverId { get; set; }

    [Required]
    public DateTime Created { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public ClaimType Type { get; set; }

    [Required]
    [Range(0, 1000000)]
    public decimal DamageCost { get; set; }
}
