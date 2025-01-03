using PersonalExpenseTracker.DTOs.Tags;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITagService
{
    /// <summary>
    /// SELECT * FROM Tags where Id = 1;
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    GetTagDto GetTagById(Guid tagId); 

    /// <summary>
    /// SELECT * FROM Tags;
    /// </summary>
    /// <returns></returns>
    Task<List<GetTagDto>> GetTags();

    Task InsertTag(InsertTagDto tag);

    Task UpdateTag(UpdateTagDto tag);

    void ActivateDeactivateTag(ActivateDeactivateTagDto tag);
}
