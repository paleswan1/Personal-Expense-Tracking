using Cashify.Domain.Models;
using Cashify.Application.Utility;
using Cashify.Application.DTOs.Tags;
using Cashify.Application.DTOs.Filters.Tags;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Repository;

namespace Cashify.Infrastructure.Implementations.Services;

/// <summary>
/// Provides services for managing tags, including retrieval, creation, updating, and activation/deactivation.
/// </summary>
/// <param name="genericRepository"> Generic repository for accessing tag data.</param>
/// <param name="userService">Service for managing user-related operations.</param>
public class TagService(IGenericRepository genericRepository, IUserService userService) : ITagService
{
    /// <summary>
    /// Retrieves a tag by its identifier.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag.</param>
    /// <returns>Details of the requested tag.</returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Retrieves all tags, with optional filtering and ordering.
    /// </summary>
    /// <param name="tagFilterRequest">Filter and order criteria for tags.</param>
    /// <returns>List of tags matching the specified criteria.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest)
    {
        
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var tags = genericRepository.GetAll<Tag>(x => x.IsDefault || x.CreatedBy == userIdentifier 
            && (string.IsNullOrEmpty(tagFilterRequest.Search) || x.Title.Contains(tagFilterRequest.Search, StringComparison.OrdinalIgnoreCase)));
        
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

    /// <summary>
    /// Inserts a new tag into the repository.
    /// </summary>
    /// <param name="tag">Details of the tag to be inserted.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Updates an existing tag in the repository.
    /// </summary>
    /// <param name="tag">Details of the tag to be updated.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Toggles the activation status of a tag.
    /// </summary>
    /// <param name="tag">Details of the tag to activate or deactivate</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ActivateDeactivateTag(ActivateDeactivateTagDto tag)
    {
        var tagModel = genericRepository.GetFirstOrDefault<Tag>(x => x.Id == tag.Id)
                       ?? throw new Exception("A tag with the following identifier couldn't be found.");

        tagModel.IsActive = !tagModel.IsActive;
        
        await genericRepository.Update(tagModel);
    }
}
