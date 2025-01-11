using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Utility;

public interface ISeedService : ISingletonService
{
    void InitializeDefaultDatasets();
}