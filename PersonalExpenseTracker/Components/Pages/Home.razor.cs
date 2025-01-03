namespace PersonalExpenseTracker.Components.Pages;

public partial class Home
{
    protected override void OnInitialized()
    {
        var userDetails = UserService.GetUserDetails();

        NavigationManager.NavigateTo(userDetails != null ? "/dashboard" : "/login");
    }
}