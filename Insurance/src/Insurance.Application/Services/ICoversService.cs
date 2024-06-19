using Insurance.Application.DTOs;
using Insurance.Application.Messages.Commands;
using Insurance.Application.Messages.Queries;

namespace Insurance.Application.Services;

public interface ICoversService
{
    //ToDo: change it to query for consistency
    Task<bool> IsDateCoveredAsync(string coverId, DateTime date);
    decimal ComputePremium(ComputePremium query);
    Task<IEnumerable<CoverDTO>> GetAllAsync(GetCovers query);
    Task<CoverDTO?> GetAsync(GetCover query);
    Task<CoverDTO> CreateAsync(CreateCover command);
    Task DeleteAsync(DeleteCover command);
}
