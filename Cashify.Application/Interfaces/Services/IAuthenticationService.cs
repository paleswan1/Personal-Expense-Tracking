using Cashify.Application.DTOs.Authentication;
using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Services;

public interface IAuthenticationService : ITransientService
{
    int GetUsersCount();

    Task Register(RegistrationRequestDto registrationRequest);

    Task Login(LoginRequestDto loginRequest);

    Task Logout();
}