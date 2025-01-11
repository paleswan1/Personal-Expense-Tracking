using Cashify.Domain.Models;
using Cashify.Application.Utility;
using Cashify.Application.DTOs.Tags;
using Cashify.Application.DTOs.Filters.Tags;
using Cashify.Application.Interfaces.Repository;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Utility;
using IUserService = Cashify.Application.Interfaces.Utility.IUserService;

namespace Cashify.Infrastructure.Implementations.Services;

public class TagService(IGenericRepository genericRepository, IUserService userService) : ITagService
{
    public GetTagDto GetTagById(Guid tagId)
    {
        var tag = genericRepository.GetFirstOrDefault<Tag>(x => x.Id == tagId)
            ?? throw new Exception("A tag with following identifier couldn't be found.");

        var result = new GetTagDto
        {
            Id = tag.Id,
            Title = tag.Title,
            BackgroundColor = tag.BackgroundColor,
            TextColor = tag.TextColor,
            IsDefault = tag.IsDefault
        };

        return result;
    }

    public async Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest)
    {
        var tags = genericRepository.GetAll<Tag>();
        
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        tags = tags.Where(x => x.IsDefault || x.CreatedBy == userIdentifier).ToList();

        if (!string.IsNullOrEmpty(tagFilterRequest.Search))
        {
            tags = tags.Where(x => x.Title.Contains(tagFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        if (!string.IsNullOrEmpty(tagFilterRequest.OrderBy))
        {
            tags = tagFilterRequest.OrderBy switch
            {
                "Title" => tagFilterRequest.IsDescending ? tags.OrderByDescending(x => x.Title).ToList() : tags.OrderBy(x => x.Title).ToList(),
                _ => tags
            };
        }

        return tags.Select(tag => new GetTagDto
        {
            Id = tag.Id,
            Title = tag.Title,
            BackgroundColor = tag.BackgroundColor,
            TextColor = tag.TextColor,
            IsDefault = tag.IsDefault
        }).ToList();
    }

    public async Task InsertTag(InsertTagDto tag)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var tagModel = new Tag
        {
            Id = Guid.NewGuid(),
            Description = tag.Description,
            Title = tag.Title,
            BackgroundColor = tag.BackgroundColor.ToHexCode(),
            TextColor = tag.TextColor.ToHexCode(),
            IsDefault = false
        };

        await genericRepository.Insert(tagModel);
    }

    public async Task UpdateTag(UpdateTagDto tag)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var tagModel = genericRepository.GetFirstOrDefault<Tag>(x => x.Id == tag.Id)
            ?? throw new Exception("A tag with the following identifier couldn't be found.");

        tagModel.Title = tag.Title;
        tagModel.Description = tag.Description;
        tagModel.BackgroundColor = tag.BackgroundColor.ToHexCode();
        tagModel.TextColor = tag.TextColor.ToHexCode();

        await genericRepository.Update(tagModel);
    }

    public async Task ActivateDeactivateTag(ActivateDeactivateTagDto tag)
    {
        var tagModel = genericRepository.GetFirstOrDefault<Tag>(x => x.Id == tag.Id)
                       ?? throw new Exception("A tag with the following identifier couldn't be found.");

        tagModel.IsActive = !tagModel.IsActive;
        
        await genericRepository.Update(tagModel);
    }
}
