using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IAuthenticationService
{
    bool IsUserRegistered();

    void Login(LoginRequestDto login); 
    
    void Register(RegisterRequestDto register);
    
    void Logout(); 
}
