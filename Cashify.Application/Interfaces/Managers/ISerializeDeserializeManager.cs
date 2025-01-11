using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Managers;

public interface ISerializeDeserializeManager : ITransientService
{
    string Serialize<T>(List<T> entity);

    List<T> Deserialize<T>(string value);
}
