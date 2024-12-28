namespace PersonalExpenseTracker.Managers;

public interface ILocalStorageManager
{
    Task<T?> GetItemAsync<T>(string key);

    Task SetItemAsync<T>(string key, T value);

    Task ClearItemAsync(string key);
}
