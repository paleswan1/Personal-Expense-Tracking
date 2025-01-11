using ExpenseTracker.Application.DTOs.Sources;
using ExpenseTracker.Application.DTOs.Filters.Sources;
using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface ISourceService : ITransientService
{
    GetSourceDto GetSourceById(Guid sourceId); 

    Task<List<GetSourceDto>> GetAllSources(GetSourceFilterRequestDto sourceFilterRequest);

    Task InsertSource(InsertSourceDto source);

    Task UpdateSource(UpdateSourceDto source);

    Task ActivateDeactivateSource(ActivateDeactivateSourceDto source);
}
