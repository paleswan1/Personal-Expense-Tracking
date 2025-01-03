using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IUserService
{
    UserDetailsDto? GetUserDetails();
}
