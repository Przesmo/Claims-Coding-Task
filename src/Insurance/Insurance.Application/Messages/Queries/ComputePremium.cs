using Insurance.Infrastructure.Repositories.Covers;

namespace Insurance.Application.Messages.Queries;

public class ComputePremium
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CoverType CoverType { get; set; }
}
