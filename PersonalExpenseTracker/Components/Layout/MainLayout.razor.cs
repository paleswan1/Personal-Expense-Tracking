using MudBlazor;
using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Components.Layout;

public partial class MainLayout
{
    public string PageTitle { get; set; } = "Personal Expense Tracker";
    
    public UserDetailsDto? UserDetails { get; set; }
    
    private bool DrawerOpen { get; set; } = true;
    
    private MudTheme Theme { get; } = new ();

    private static bool RightToLeft => false;

    protected override void OnInitialized()
    {
        var userDetails = UserService.GetUserDetails();

        if (userDetails != null)
        {
            UserDetails = userDetails;
        }
        else
        {
            NavigationManager.NavigateTo("/login");
        }
    }

    private void LogoutHandler()
    {
        AuthenticationService.Logout();

        NavigationManager.NavigateTo("/login", true);
    }

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
}