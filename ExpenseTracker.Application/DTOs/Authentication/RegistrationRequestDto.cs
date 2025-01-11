using ExpenseTracker.Domain.Common.Enum;

namespace ExpenseTracker.Application.DTOs.Authentication;

public class RegistrationRequestDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Currency Currency { get; set; } = Currency.None;
}