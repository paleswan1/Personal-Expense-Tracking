using MudBlazor;
using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.Components.Layout;
using PersonalExpenseTracker.DTOs.Authentication;

namespace PersonalExpenseTracker.Components.Pages.Authentication;

public partial class Login
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
    private LoginRequestDto LoginDetails { get; set; } = new();

    private async Task LoginHandler()
    {
        try
        {
            await AuthenticationService.Login(LoginDetails);
            
            SnackbarService.ShowSnackbar("User successfully logged in.", Severity.Success, Variant.Outlined);

            NavigationManager.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}