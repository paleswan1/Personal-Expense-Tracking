using ExpenseTracker.Application.Interfaces.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Infrastructure.Dependency;

public static class MigrationService
{
    public static void AddDataSeedMigrationService(this IServiceCollection services)
    {
        SeedDefaultDataSets(services);
    }
    
    private static void SeedDefaultDataSets(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();

        var dbInitializer = scope.ServiceProvider.GetRequiredService<ISeedService>();

        dbInitializer.InitializeDefaultDatasets();
    }
}