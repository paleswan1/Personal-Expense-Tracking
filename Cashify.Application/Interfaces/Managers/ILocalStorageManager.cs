using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Managers;

public interface ILocalStorageManager : ITransientService
{
    Task<T?> GetItemAsync<T>(string key);

    Task SetItemAsync<T>(string key, T value);

    Task ClearItemAsync(string key);
}
