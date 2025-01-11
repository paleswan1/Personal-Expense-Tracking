using Cashify.Application.DTOs.Authentication;
using Cashify.Components.Layout;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cashify.Components.Pages.Authentication;

public partial class Register
{
    protected override void OnInitialized()
    {
        SetPageTitle();
    }

    #region Page Title

    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Login";
    }

    #endregion

    #region User Registration
    private RegistrationRequestDto RegistrationRequest { get; set; } = new();

    private void RegisterHandler()
    {
        try
        {
            AuthenticationService.Register(RegistrationRequest);
            
            SnackbarService.ShowSnackbar("User successfully registered.", Severity.Success, Variant.Outlined);
            
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}