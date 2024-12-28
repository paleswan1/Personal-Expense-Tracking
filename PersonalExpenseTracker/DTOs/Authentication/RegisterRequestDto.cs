namespace PersonalExpenseTracker.DTOs.Authentication;

public class RegisterRequestDto
{
    public string Name { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string Currency { get; set; }
}
