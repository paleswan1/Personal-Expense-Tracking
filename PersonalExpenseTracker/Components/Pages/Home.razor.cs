namespace PersonalExpenseTracker.Components.Pages;

public partial class Home
{
    protected override async Task OnInitializedAsync()
    {
        var userDetails = await UserService.GetUserDetails();

        if (userDetails != null)
        {
            NavigationManager.NavigateTo("/dashboard");
            
            return;
        }

        var users = UserService.GetAllUsers();
        
        NavigationManager.NavigateTo(users.Count > 0 ? "/login" : "/register");
    }
}