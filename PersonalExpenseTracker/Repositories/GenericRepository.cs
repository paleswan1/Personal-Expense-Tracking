using System.Text.Json;
using PersonalExpenseTracker.Managers;

namespace PersonalExpenseTracker.Repositories;

public class GenericRepository(ISerializeDeserializeManager serializeDeserializeManager) : IGenericRepository
{
    public List<T> GetAll<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(filePath);
        
        var result = serializeDeserializeManager.Deserialize<T>(json);

        return result;
    }

    public void SaveAll<T>(List<T> entity, string directoryPath, string filePath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var result = serializeDeserializeManager.Serialize<T>(entity);

        File.WriteAllText(filePath, result);    
    }
}
