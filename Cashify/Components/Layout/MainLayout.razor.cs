using Cashify.Application.DTOs.Theme;
using Cashify.Application.DTOs.User;
using MudBlazor;

namespace Cashify.Components.Layout;

public partial class MainLayout
{
    protected override async Task OnInitializedAsync()
    {
        await GetUserDetails();
    }

    #region Page Title
    public string PageTitle { get; set; } = "Cashify.";
    #endregion

    #region User Details
    private GetUserDetailsDto UserDetails { get; set; } = new();
    
    private async Task GetUserDetails()
    {
        var userIdentifier = await UserService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            var users = AuthenticationService.GetUsersCount();
        
            NavigationManager.NavigateTo(users > 0 ? "/login" : "/register");
            
            return;
        }
        
        UserDetails = await ProfileService.GetUserDetails();
    }
    #endregion

    #region Layout and Themes
    private MudTheme Theme { get; } = new LightTheme();

    private bool DrawerOpen { get; set; } = true;

    private static bool RightToLeft => false;

    private void DrawerToggle()
    {
        DrawerOpen = !DrawerOpen;
    }
    #endregion

    #region Logout Functionality
    private void LogoutHandler()
    {
        AuthenticationService.Logout();

        SnackbarService.ShowSnackbar("User successfully logged out.", Severity.Success, Variant.Outlined);
        
        NavigationManager.NavigateTo("/login", true);
    }
    #endregion
}