namespace Cashify.Components.Layout;

public partial class NavMenu
{
    private async Task Logout()
    {
        await AuthenticationService.Logout();

        NavigationManager.NavigateTo("/login");
    }
}