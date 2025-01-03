using PersonalExpenseTracker.DTOs.Debts;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IDebtService
{
    GetDebtDto GetDebtById(Guid id);

    /// <summary>
    /// SELECT * FROM Debts;
    /// </summary>
    /// <returns></returns>
    List<GetDebtDto> GetDebts();

    void InsertDebt(InsertDebtDto debt);

    void UpdateDebt(UpdateDebtDto transaction);

    void ClearDebt(Guid debtId);

    void ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
