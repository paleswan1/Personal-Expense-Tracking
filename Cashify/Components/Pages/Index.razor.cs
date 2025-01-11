namespace Cashify.Components.Pages;

public partial class Index
{
    protected override async Task OnInitializedAsync()
    {
        await GetNavigationUrl();
    }

    private async Task GetNavigationUrl()
    {
        var userIdentifier = await UserService.GetUserId();

        if (userIdentifier != Guid.Empty)
        {
            NavigationManager.NavigateTo("/dashboard");

            return;
        }

        var users = AuthenticationService.GetUsersCount();

        NavigationManager.NavigateTo(users > 0 ? "/login" : "/register");
    }
}