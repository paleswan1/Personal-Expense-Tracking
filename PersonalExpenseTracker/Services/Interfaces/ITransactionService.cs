using PersonalExpenseTracker.DTOs.Transaction;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITransactionService
{
    GetTransactionDto GetTransactionById(Guid transactionId);

    /// <summary>
    /// SELECT * FROM Transactions;
    /// </summary>
    /// <returns></returns>
    List<GetTransactionDto> GetTransactions();

    void InsertTransaction(InsertTransactionDto transaction);

    void UpdateTransaction(UpdateTransactionDto transaction);

    void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
