using Cashify.Domain.Models;
using Cashify.Application.DTOs.User;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Repository;

namespace Cashify.Infrastructure.Implementations.Services;

/// <summary>
/// Provides services for managing and retrieving user profile details.
/// </summary>
/// <param name="genericRepository">Generic repository for database operations.</param>
/// <param name="userService">Service to retrieve the current user's details.</param>
public class ProfileService(IGenericRepository genericRepository, IUserService userService) : IProfileService
{
    /// <summary>
    /// Retrieves details of the currently logged-in user.
    /// </summary>
    /// <returns> An object containing user details such as ID, username, and currency.</returns>
    /// <exception cref="Exception">Thrown if the user cannot be found in the system.</exception</exception>
    public async Task<GetUserDetailsDto> GetUserDetails()
    {
        var userIdentifier = await userService.GetUserId();

        var userModel = genericRepository.GetFirstOrDefault<User>(x => x.Id == userIdentifier)
                        ?? throw new Exception("The following user could not be found.");

        return new GetUserDetailsDto()
        {
            Id = userModel.Id,
            Username = userModel.Username,
            Currency = userModel.Currency.ToString()
        };
    }
}