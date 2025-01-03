using Blazored.LocalStorage;
using MudBlazor;
using MudBlazor.Services;

namespace PersonalExpenseTracker.Services.Dependency;

public static class InfrastructureService
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        services.AddLocalization();

        services.AddMauiBlazorWebView();

        services.AddBlazoredLocalStorage();
        
        services.AddBlazorWebViewDeveloperTools();

        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
        });
    }
}