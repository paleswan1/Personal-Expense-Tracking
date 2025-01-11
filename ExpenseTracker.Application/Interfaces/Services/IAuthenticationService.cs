using ExpenseTracker.Application.DTOs.Authentication;
using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IAuthenticationService : ITransientService
{
    int GetUsersCount();

    Task Register(RegistrationRequestDto registrationRequest);

    Task Login(LoginRequestDto loginRequest);

    Task Logout();
}