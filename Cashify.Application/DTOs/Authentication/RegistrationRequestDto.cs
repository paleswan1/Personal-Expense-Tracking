using Cashify.Domain.Common.Enum;

namespace Cashify.Application.DTOs.Authentication;

public class RegistrationRequestDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Currency Currency { get; set; } = Currency.None;
}