namespace Insurance.Application.Exceptions;

public class ClaimNotCoveredException : Exception
{
    public ClaimNotCoveredException(string coverId, DateTime notCoveredDate)
        : base($"Claim with coverId: {coverId} is not covered for the date: {notCoveredDate:dd/MM/yyyy}")
    {
    }
}
