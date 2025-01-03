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
    List<GetTagDto> GetTags();

    void InsertTag(InsertTagDto tag);

    void UpdateTag(UpdateTagDto tag);

    void ActivateDeactivateTag(ActivateDeactivateTagDto tag);
}
