using PersonalExpenseTracker.DTOs.Debts;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface IDebtService
{
    GetDebtDto GetDebtById(Guid Id);

    /// <summary>
    /// Select * from tag;
    /// </summary>
    /// <returns></returns>
    List<GetDebtDto> GetTransactions();

    void InsertDebt(InsertDebtDto debt);

    void UpdateDebt(UpdateDebtDto transaction);

    void ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
