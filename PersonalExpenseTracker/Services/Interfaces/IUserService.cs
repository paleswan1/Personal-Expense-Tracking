using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IUserService
{
    Task<UserDetailsDto?> GetUserDetails();

    List<UserDetailsDto> GetAllUsers();
}
