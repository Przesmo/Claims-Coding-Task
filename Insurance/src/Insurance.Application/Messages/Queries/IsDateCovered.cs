namespace Insurance.Application.Messages.Queries;

public class IsDateCovered
{
    public required string CoverId { get; set; }
    public required DateTime DateToCover { get; set; }
}
