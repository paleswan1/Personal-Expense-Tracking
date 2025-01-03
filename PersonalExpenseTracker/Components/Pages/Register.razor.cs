using Microsoft.AspNetCore.Components;
using MudBlazor;
using PersonalExpenseTracker.Components.Layout;
using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Components.Pages;

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
    private RegisterRequestDto Registration { get; set; } = new();

    private void RegisterHandler()
    {
        try
        {
            AuthenticationService.Register(Registration);
            
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