using Cashify.Application.DTOs.User;
using Cashify.Application.Interfaces.Repository;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Utility;
using Cashify.Domain.Models;

namespace Cashify.Infrastructure.Implementations.Services;

public class ProfileService(IGenericRepository genericRepository, IUserService userService) : IProfileService
{
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