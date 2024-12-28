using Blazored.LocalStorage;

namespace PersonalExpenseTracker.Managers;

public class LocalStorageManager(ILocalStorageService localStorage) : ILocalStorageManager
{
    /// <summary>
    /// Retrieves data stored in local storage(stores value in key value pair and fetches data) based on the provided keys
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<T?> GetItemAsync<T>(string key)
    {
        return await localStorage.GetItemAsync<T>(key); //<T> generic data type
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var item = await GetItemAsync<T>(key);

        if (item != null) await ClearItemAsync(key); // Checks if item is null, if not null removes exixting data stored in that particular key.

        await localStorage.SetItemAsync(key, value);
    }

    public async Task ClearItemAsync(string key)
    {
        await localStorage.RemoveItemAsync(key);
    }
}
