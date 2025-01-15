using Cashify.Domain.Models;
using Cashify.Application.Utility;
using Cashify.Application.DTOs.Sources;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.DTOs.Filters.Sources;
using Cashify.Application.Interfaces.Repository;
using IUserService = Cashify.Application.Interfaces.Utility.IUserService;

namespace Cashify.Infrastructure.Implementations.Services;

/// <summary>
/// Provides services for managing debt sources, including retrieving, creating, updating, and activating/deactivating sources.
/// </summary>
/// <param name="genericRepository"> Generic repository for data access.</param>
/// <param name="userService">Service for retrieving user-related information.</param>
public class SourceService(IGenericRepository genericRepository, IUserService userService) : ISourceService
{
    /// <summary>
    /// REtrieves a source by its identifier
    /// </summary>
    /// <param name="sourceId">The unique identifier of the source.</param>
    /// <returns>Details of the requested source.</returns>
    /// <exception cref="Exception"></exception>
    public GetSourceDto GetSourceById(Guid sourceId)
    {
        var source = genericRepository.GetFirstOrDefault<DebtSource>(x => x.Id == sourceId)
            ?? throw new Exception("A source with following identifier couldn't be found.");

        var result = new GetSourceDto
        {
            Id = source.Id,
            Title = source.Title,
            BackgroundColor = source.BackgroundColor,
            TextColor = source.TextColor
        };

        return result;
    }

    /// <summary>
    /// Retrieves all sources with optional filtering and ordering.
    /// </summary>
    /// <param name="sourceFilterRequest">Filter and order criteria for sources.</param>
    /// <returns>List of sources matching the specified criteria.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<GetSourceDto>> GetAllSources(GetSourceFilterRequestDto sourceFilterRequest)
    {
        var sources = genericRepository.GetAll<DebtSource>();
        
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        sources = sources.Where(x => x.CreatedBy == userIdentifier).ToList();

        if (!string.IsNullOrEmpty(sourceFilterRequest.Search))
        {
            sources = sources.Where(x => x.Title.Contains(sourceFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        if (!string.IsNullOrEmpty(sourceFilterRequest.OrderBy))
        {
            sources = sourceFilterRequest.OrderBy switch
            {
                "Title" => sourceFilterRequest.IsDescending ? sources.OrderByDescending(x => x.Title).ToList() : sources.OrderBy(x => x.Title).ToList(),
                _ => sources
            };
        }

        return sources.Select(source => new GetSourceDto
        {
            Id = source.Id,
            Title = source.Title,
            BackgroundColor = source.BackgroundColor,
            TextColor = source.TextColor
        }).ToList();
    }

    /// <summary>
    /// Inserts a new source into the repository
    /// </summary>
    /// <param name="source">Details of the source to insert.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task InsertSource(InsertSourceDto source)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var sourceModel = new DebtSource
        {
            Id = Guid.NewGuid(),
            Description = source.Description,
            Title = source.Title,
            BackgroundColor = source.BackgroundColor.ToHexCode(),
            TextColor = source.TextColor.ToHexCode()
        };

        await genericRepository.Insert(sourceModel);
    }

    /// <summary>
    /// Updates an existing source in the repository
    /// </summary>
    /// <param name="source">Details of the source to update.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task UpdateSource(UpdateSourceDto source)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var sourceModel = genericRepository.GetFirstOrDefault<DebtSource>(x => x.Id == source.Id)
            ?? throw new Exception("A source with the following identifier couldn't be found.");

        sourceModel.Title = source.Title;
        sourceModel.Description = source.Description;
        sourceModel.BackgroundColor = source.BackgroundColor.ToHexCode();
        sourceModel.TextColor = source.TextColor.ToHexCode();

        await genericRepository.Update(sourceModel);
    }

    /// <summary>
    /// Toggles the activation status of a source.
    /// </summary>
    /// <param name="source">Details of the source to activate or deactivate.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ActivateDeactivateSource(ActivateDeactivateSourceDto source)
    {
        var sourceModel = genericRepository.GetFirstOrDefault<DebtSource>(x => x.Id == source.Id)
                       ?? throw new Exception("A source with the following identifier couldn't be found.");

        sourceModel.IsActive = !sourceModel.IsActive;
        
        await genericRepository.Update(sourceModel);
    }
}
