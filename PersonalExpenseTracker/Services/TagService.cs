using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Filters.Tags;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TagService(IGenericRepository genericRepository, IUserService userService) : ITagService
{
    public GetTagDto GetTagById(Guid tagId)
    {
        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tag = tags.FirstOrDefault(x => x.Id == tagId);

        if (tag == null)
        {
            throw new Exception("A tag with following identifier couldn't be found.");
        }

        var result = new GetTagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            BackgroundColor = tag.BackgroundColor,
            TextColor = tag.TextColor,
        };

        return result;
    }

    public async Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest)
    {
        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);
        
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        tags = tags.Where(x => x.IsDefault || x.CreatedBy == userDetails.Id).ToList();

        if (!string.IsNullOrEmpty(tagFilterRequest.Search))
        {
            tags = tags.Where(x => x.Name.Contains(tagFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        if (!string.IsNullOrEmpty(tagFilterRequest.OrderBy))
        {
            tags = tagFilterRequest.OrderBy switch
            {
                "Name" => tagFilterRequest.IsDescending ? tags.OrderByDescending(x => x.Name).ToList() : tags.OrderBy(x => x.Name).ToList(),
                _ => tags
            };
        }
        
        // Initialization of GetTagDto List
        var result = new List<GetTagDto>();

        // Iterating through all records and data of tags
        foreach (var tag in tags)
        {
            // Addition of new data transfer object to the result list.
            result.Add(new GetTagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                BackgroundColor = tag.BackgroundColor,
                TextColor = tag.TextColor
            }); 
        }

        return result;
    }

    public async Task InsertTag(InsertTagDto tag)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var tagModel = new Tag
        {
            Id = Guid.NewGuid(), 
            Name = tag.Name,
            BackgroundColor = tag.BackgroundColor,
            TextColor = tag.TextColor,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        tags.Add(tagModel);

        genericRepository.SaveAll(tags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }

    public async Task UpdateTag(UpdateTagDto tag)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tagModel = tags.FirstOrDefault(x => x.Id == tag.Id);

        if (tagModel == null)
        {
            throw new Exception("A tag with the following identifier couldn't be found.");
        }

        tagModel.Name = tag.Name;
        tagModel.BackgroundColor = tag.BackgroundColor;
        tagModel.TextColor = tag.TextColor;
        tagModel.LastModifiedBy = userDetails.Id;
        tagModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(tags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }

    public void ActivateDeactivateTag(ActivateDeactivateTagDto tag)
    {
        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tagModel = tags.FirstOrDefault(x => x.Id == tag.Id);

        if (tagModel == null)
        {
            throw new Exception("A tag with the following identifier couldn't be found.");
        }

        tags.Remove(tagModel);

        genericRepository.SaveAll(tags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }
}
