using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IAuthenticationService
{
    bool IsUserRegistered();

    Task Login(LoginRequestDto login); 
    
    void Register(RegisterRequestDto register);
    
    void Logout(); 
}
