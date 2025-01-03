using Microsoft.Extensions.Logging;
using PersonalExpenseTracker.Services.Dependency;

namespace PersonalExpenseTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        var services = builder.Services;
            
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
            
        builder.Logging.AddDebug();

        services.AddInfrastructureService();

        return builder.Build();
    }
}