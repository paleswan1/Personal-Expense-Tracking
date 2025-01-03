using PersonalExpenseTracker.DTOs.Debts;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IDebtService
{
    GetDebtDto GetDebtById(Guid id);

    /// <summary>
    /// SELECT * FROM Debts;
    /// </summary>
    /// <returns></returns>
    Task<List<GetDebtDto>> GetDebts();

    Task InsertDebt(InsertDebtDto debt);

    Task UpdateDebt(UpdateDebtDto transaction);

    Task ClearDebt(Guid debtId);

    Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
