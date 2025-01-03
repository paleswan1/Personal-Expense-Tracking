using PersonalExpenseTracker.DTOs.Authentication;
using PersonalExpenseTracker.DTOs.Balance;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IUserService
{
    UserDetailsDto? GetUserDetails();

    void UpdateBalance(UpdateBalanceDto balance);

}
