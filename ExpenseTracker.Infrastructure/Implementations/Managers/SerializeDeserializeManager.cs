using System.Text.Json;
using ExpenseTracker.Application.Interfaces.Managers;

namespace ExpenseTracker.Infrastructure.Implementations.Managers;

public class SerializeDeserializeManager : ISerializeDeserializeManager
{
    public string Serialize<T>(List<T> entity)
    {
        var json = JsonSerializer.Serialize(entity);

        return json;
    }

    public List<T> Deserialize<T>(string value)
    {
        var result = JsonSerializer.Deserialize<List<T>>(value);

        return result ?? new List<T>();
    } 
}

