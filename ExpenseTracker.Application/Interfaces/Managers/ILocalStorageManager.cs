using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Managers;

public interface ILocalStorageManager : ITransientService
{
    Task<T?> GetItemAsync<T>(string key);

    Task SetItemAsync<T>(string key, T value);

    Task ClearItemAsync(string key);
}
