using Cashify.Application.DTOs.User;
using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Services;

public interface IProfileService : ITransientService
{
    Task<GetUserDetailsDto> GetUserDetails();
}