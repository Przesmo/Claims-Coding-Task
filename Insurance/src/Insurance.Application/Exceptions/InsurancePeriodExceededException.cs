namespace Insurance.Application.Exceptions;

public class InsurancePeriodExceededException : Exception
{
    public InsurancePeriodExceededException(int maximumInsurnacePeriodDays)
        : base($"Maximum insurance period of {maximumInsurnacePeriodDays} days has been exceeded")
    {
    }
}
