using MudBlazor;
using System.Reflection;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.Maui.Hosting;
using System.IdentityModel.Tokens.Jwt;
using Cashify.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cashify.Infrastructure.Dependency;

public static class InfrastructureService
{
    public static void AddInfrastructureService(this IServiceCollection services, MauiAppBuilder builder)
    {
        #region Configurations for Application Settings
        var assembly = Assembly.GetEntryAssembly();
        
        using var stream = assembly?.GetManifestResourceStream("Cashify.appsettings.json");

        if (stream != null)
        {
            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }
        
        services.AddSingleton<JwtSecurityTokenHandler>();
        #endregion
        
        #region Injection of Internal Services
        services.AddLocalization();

        services.AddBlazoredLocalStorage();
        #endregion
        
        #region Registration of DI Containers
        services.AddDependencyServices();
        #endregion

        #region MudBlazor Services
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

        #region Authentication Core
        services.AddAuthorizationCore();
        #endregion

        #region Migration and Seeding Service
        services.AddDataSeedMigrationService();
        #endregion
    }
}