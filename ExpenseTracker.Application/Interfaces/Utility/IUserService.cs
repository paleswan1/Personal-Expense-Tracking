using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Utility;

public interface IUserService : ITransientService
{
    Task<Guid> GetUserId();
}