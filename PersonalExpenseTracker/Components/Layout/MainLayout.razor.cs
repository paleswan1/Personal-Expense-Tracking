using MudBlazor;
using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Components.Layout;

public partial class MainLayout
{
    public string PageTitle { get; set; } = "Personal Expense Tracker";
    
    private UserDetailsDto? UserDetails { get; set; }
    
    private bool DrawerOpen { get; set; } = true;
    
    private MudTheme Theme { get; } = new ();

    private static bool RightToLeft => false;

    protected override async Task OnInitializedAsync()
    {
        await GetUserDetails();
    }

    public async Task GetUserDetails()
    {
        var userDetails = await UserService.GetUserDetails();

        if (userDetails != null)
        {
            UserDetails = userDetails;
        }
        else
        {
            var users = UserService.GetAllUsers();
        
            NavigationManager.NavigateTo(users.Count > 0 ? "/login" : "/register");
        }
    }
    
    private void LogoutHandler()
    {
        AuthenticationService.Logout();

        SnackbarService.ShowSnackbar("User successfully logged out.", Severity.Success, Variant.Outlined);
        
        NavigationManager.NavigateTo("/login", true);
    }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}