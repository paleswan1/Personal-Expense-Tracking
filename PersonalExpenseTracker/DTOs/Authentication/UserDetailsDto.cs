namespace PersonalExpenseTracker.DTOs.Authentication;

public class UserDetailsDto
{
    public Guid Id { get; set; }

    public string Name {  get; set; }

    public string UserName{ get; set; }

    public string Currency { get; set; }
}
