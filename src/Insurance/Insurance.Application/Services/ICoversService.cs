namespace Insurance.Application.Services;

public interface ICoversService
{
    Task<bool> IsDateCoveredAsync(string coverId, DateTime date);
}
