using Blazored.LocalStorage;
using Cashify.Application.Interfaces.Managers;

namespace Cashify.Infrastructure.Implementations.Managers;

public class LocalStorageManager(ILocalStorageService localStorage) : ILocalStorageManager
{
    /// <summary>
    /// Retrieves an item from local storage by key
    /// </summary>
    /// <typeparam name="T">The type of the item to be retrieved.</typeparam>
    /// <param name="key">The key associated with the item in local storage</param>
    /// <returns></returns>
    public async Task<T?> GetItemAsync<T>(string key)
    {
        return await localStorage.GetItemAsync<T>(key);
    }

    /// <summary>
    /// Stores an item in local storage with the specified key.
    /// If it already exists, it is removed before storing the new item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>

    public async Task SetItemAsync<T>(string key, T value)
    {
        var item = await GetItemAsync<T>(key);

        if (item != null) await ClearItemAsync(key);

        await localStorage.SetItemAsync(key, value);
    }

    /// <summary>
    /// Removes an item from local storage by key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task ClearItemAsync(string key)
    {
        await localStorage.RemoveItemAsync(key);
    }
}
