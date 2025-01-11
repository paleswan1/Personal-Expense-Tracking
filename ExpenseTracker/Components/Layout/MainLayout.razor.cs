using ExpenseTracker.Application.DTOs.User;

namespace ExpenseTracker.Components.Layout;

public partial class MainLayout
{
    protected override async Task OnInitializedAsync()
    {
        await GetUserId();
    }
    
    private async Task GetUserId()
    {
        var userIdentifier = await UserService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            var users = AuthenticationService.GetUsersCount();
        
            NavigationManager.NavigateTo(users > 0 ? "/login" : "/register");
        }
    }
}