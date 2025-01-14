using System.Text.Json;
using Cashify.Application.Interfaces.Managers;

namespace Cashify.Infrastructure.Implementations.Managers;

public class SerializeDeserializeManager : ISerializeDeserializeManager
{
    /// <summary>
    /// Serializes list of entity into a JSON string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string Serialize<T>(List<T> entity)
    {
        var json = JsonSerializer.Serialize(entity);

        return json;
    }
    /// <summary>
    /// Deserializes a JSON string into a list of entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public List<T> Deserialize<T>(string value)
    {
        var result = JsonSerializer.Deserialize<List<T>>(value);

        return result ?? new List<T>();
    } 
}

