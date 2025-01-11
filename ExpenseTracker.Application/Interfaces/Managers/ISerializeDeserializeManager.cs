using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Managers;

public interface ISerializeDeserializeManager : ITransientService
{
    string Serialize<T>(List<T> entity);

    List<T> Deserialize<T>(string value);
}
