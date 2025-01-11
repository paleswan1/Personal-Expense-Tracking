namespace Cashify.Application.DTOs.User;

public class GetUserDetailsDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;
}
