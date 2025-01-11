using MudBlazor;
using Cashify.Application.DTOs.Authentication;

namespace Cashify.Components.Pages.Authentication;

public partial class Login
{
    private LoginRequestDto LoginRequest { get; set; } = new();

    private async Task LoginHandler()
    {
        try
        {
            await AuthenticationService.Login(LoginRequest);
            
            SnackbarService.ShowSnackbar("User successfully logged in.", Severity.Success, Variant.Outlined);
            
            NavigationManager.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }

    #region Navigate to Registration View
    private void HandleRegister()
    {
        NavigationManager.NavigateTo("/register");
    }
    #endregion
}