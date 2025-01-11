using ExpenseTracker.Application.DTOs.Debts;
using ExpenseTracker.Application.DTOs.Filters.Debts;
using ExpenseTracker.Application.Interfaces.Dependency;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IDebtService : ITransientService
{
    Task<decimal> GetPendingDebtAmounts();

    Task<GetDebtsCountDto> GetDebtsCount();
    
    GetDebtDto GetDebtById(Guid id);

    Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest);

    Task InsertDebt(InsertDebtDto debt);

    Task UpdateDebt(UpdateDebtDto transaction);

    Task ClearDebt(Guid debtId);

    Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
