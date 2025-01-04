using PersonalExpenseTracker.DTOs.Debts;
using PersonalExpenseTracker.Filters.Debts;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IDebtService
{
    Task<decimal> GetRemainingBalance();

    Task<decimal> GetPendingDebtAmounts();

    Task<GetDebtsCountDto> GetDebtsCount();
    
    GetDebtDto GetDebtById(Guid id);

    /// <summary>
    /// SELECT * FROM Debts;
    /// </summary>
    /// <returns></returns>
    Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest);

    Task InsertDebt(InsertDebtDto debt);

    Task UpdateDebt(UpdateDebtDto transaction);

    Task ClearDebt(Guid debtId);

    Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
