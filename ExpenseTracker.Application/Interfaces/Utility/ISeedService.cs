using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Utility;

public interface ISeedService : ISingletonService
{
    void InitializeDefaultDatasets();
}