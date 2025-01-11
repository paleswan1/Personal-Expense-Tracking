using MudBlazor;
using Cashify.Application.DTOs.Authentication;

namespace Cashify.Components.Pages.Authentication;

public partial class Register
{
    private RegistrationRequestDto RegistrationRequest { get; set; } = new();
    
    private async Task RegistrationHandler()
    {
        try
        {
            await AuthenticationService.Register(RegistrationRequest);
            
            SnackbarService.ShowSnackbar("User successfully registered.", Severity.Success, Variant.Outlined);
            
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    
}