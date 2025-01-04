using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using PersonalExpenseTracker.Managers;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;
using PersonalExpenseTracker.Services.Seed;

namespace PersonalExpenseTracker.Services.Dependency;

public static class InfrastructureService
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        #region Inject of Internal Services
        services.AddLocalization();

        services.AddMauiBlazorWebView();

        services.AddBlazoredLocalStorage();
        
        services.AddBlazorWebViewDeveloperTools();
        #endregion

        #region Mudblazor Service
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
        #endregion

        #region Dependency Injection
        services.AddTransient<ILocalStorageManager, LocalStorageManager>();
        services.AddTransient<ISerializeDeserializeManager, SerializeDeserializeManager>();

        services.AddTransient<IGenericRepository, GenericRepository>();

        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IDebtService, DebtService>();
        services.AddTransient<ISeedService, SeedService>();
        services.AddTransient<ISnackbarService, SnackbarService>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<ITransactionService, TransactionService>();
        services.AddTransient<IUserService, UserService>();
        #endregion
    }
}