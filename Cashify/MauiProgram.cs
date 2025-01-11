using Microsoft.Extensions.Logging;
using Cashify.Infrastructure.Dependency;

namespace Cashify;

/// <summary>
/// Startup Class for a MAUI Project and Program File.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Initializes the MAUI Project with all of its Dependencies and Configurations.
    /// </summary>
    /// <returns></returns>
    public static MauiApp CreateMauiApp()
    {
        // Builder Setup
        var builder = MauiApp.CreateBuilder();
        
        // Instantiating the MAUI Application
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // Instantiating the Logging and Services Mechanism
        var logging = builder.Logging;
        var services = builder.Services;
        
        // Addition of a Logger for Debugging Procedure
        logging.AddDebug();
        
        // Setting up and Injecting Respective Services
        services.AddMauiBlazorWebView();
        services.AddBlazorWebViewDeveloperTools();

        // Injection of Infrastructure Services
        services.AddInfrastructureService(builder);

        return builder.Build();
    }
}