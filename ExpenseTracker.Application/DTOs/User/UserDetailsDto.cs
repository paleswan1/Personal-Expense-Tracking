namespace ExpenseTracker.Application.DTOs.User;

public class UserDetailsDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;
}
