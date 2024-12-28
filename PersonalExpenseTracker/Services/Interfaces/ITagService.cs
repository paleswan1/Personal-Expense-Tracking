using PersonalExpenseTracker.DTOs.Tags;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITagService
{
    /// <summary>
    /// Select * from Tag where tag_id = 1;
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    GetTagDto GetTagById(Guid tagId); 

    /// <summary>
    /// Select * from tag;
    /// </summary>
    /// <returns></returns>
    List<GetTagDto> GetTags();

    void InsertTag(InsertTagDto tag);

    void UpdateTag(UpdateTagDto tag);

    void ActivateDeactivateTag(ActivateDeactivateTagDto tag);


}
