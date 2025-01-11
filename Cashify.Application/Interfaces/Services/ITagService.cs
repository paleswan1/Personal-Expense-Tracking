using Cashify.Application.DTOs.Tags;
using Cashify.Application.DTOs.Filters.Tags;
using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Services;

public interface ITagService : ITransientService
{
    GetTagDto GetTagById(Guid tagId); 

    Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest);

    Task InsertTag(InsertTagDto tag);

    Task UpdateTag(UpdateTagDto tag);

    Task ActivateDeactivateTag(ActivateDeactivateTagDto tag);
}
