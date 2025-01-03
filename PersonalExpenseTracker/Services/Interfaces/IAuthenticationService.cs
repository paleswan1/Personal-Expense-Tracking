using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IAuthenticationService
{
    bool IsUserRegistered();

    void Register(RegisterRequestDto register);

    void Login(LoginRequestDto login); 
}
