using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TagService(IGenericRepository genericRepository, IUserService userService) : ITagService
{
    private static string appDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
    private static string appTagsFilePath = ExtensionMethods.GetAppTagsFilePath();

    public GetTagDto GetTagById(Guid tagId)
    {
        var tags = genericRepository.GetAll<Tag>(appTagsFilePath);

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

    public List<GetTagDto> GetTags()
    {
        var tags = genericRepository.GetAll<Tag>(appTagsFilePath);
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        tags = tags.Where(x => x.IsDefault || x.CreatedBy == userDetails.Id).ToList();
        //return tags.Select(tag => new GetTagDto
        //{
        //    Id = tag.Id,
        //    Name = tag.Name,
        //    BackgroundColor = tag.BackgroundColor,
        //    TextColor = tag.TextColor
        //}).ToList();

        // Initialization of GetTagDto list
        var result = new List<GetTagDto>();

        // Iterating through all records of tags
        foreach (var tag in tags)
        {
            // Addition of new GetTagDto object to the result list.
            result.Add(new GetTagDto()
            {
                Id = tag.Id,
                Name = tag.Name,
                BackgroundColor = tag.BackgroundColor,
                TextColor = tag.TextColor
            }); 
        }

        return result;
    }

    public void InsertTag(InsertTagDto tag)
    {
        var userDetails = userService.GetUserDetails();

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

        var tags = genericRepository.GetAll<Tag>(appTagsFilePath);

        tags.Add(tagModel);

        genericRepository.SaveAll(tags, appDataDirectoryPath, appTagsFilePath);
    }

    public void UpdateTag(UpdateTagDto tag)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var tags = genericRepository.GetAll<Tag>(appTagsFilePath);

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

        genericRepository.SaveAll(tags,appDataDirectoryPath, appTagsFilePath);
    }

    public void ActivateDeactivateTag(ActivateDeactivateTagDto tag)
    {
        var tags = genericRepository.GetAll<Tag>(appTagsFilePath);

        var tagModel = tags.FirstOrDefault(x => x.Id == tag.Id);

        if (tagModel == null)
        {
            throw new Exception("A tag with the following identifier couldn't be found.");
        }

        tags.Remove(tagModel);

        genericRepository.SaveAll(tags,appDataDirectoryPath, appTagsFilePath);
    }
}
