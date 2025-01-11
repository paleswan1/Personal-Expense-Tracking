using Cashify.Application.DTOs.Sources;
using Cashify.Application.DTOs.Filters.Sources;
using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Services;

public interface ISourceService : ITransientService
{
    GetSourceDto GetSourceById(Guid sourceId); 

    Task<List<GetSourceDto>> GetAllSources(GetSourceFilterRequestDto sourceFilterRequest);

    Task InsertSource(InsertSourceDto source);

    Task UpdateSource(UpdateSourceDto source);

    Task ActivateDeactivateSource(ActivateDeactivateSourceDto source);
}
