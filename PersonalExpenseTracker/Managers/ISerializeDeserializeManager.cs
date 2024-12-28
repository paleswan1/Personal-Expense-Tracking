namespace PersonalExpenseTracker.Managers;

public interface ISerializeDeserializeManager
{
    string Serialize<T>(List<T> entity);

    List<T> Deserialize<T>(string value);
}
