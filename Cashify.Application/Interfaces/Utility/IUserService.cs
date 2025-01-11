using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Utility;

public interface IUserService : ITransientService
{
    Task<Guid> GetUserId();
}