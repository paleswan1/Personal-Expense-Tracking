using PersonalExpenseTracker.DTOs.Transaction;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITransactionService
{
    GetTransactionDto GetTransactionById(Guid transactionId);

    /// <summary>
    /// Select * from tag;
    /// </summary>
    /// <returns></returns>
    List<GetTransactionDto> GetTransactions();

    void InsertTransaction(InsertTransactionDto transaction);

    void UpdateTrasaction(UpdateTransactionDto transaction);

    void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
