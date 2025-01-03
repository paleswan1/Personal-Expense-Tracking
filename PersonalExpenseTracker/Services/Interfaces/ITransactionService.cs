using PersonalExpenseTracker.DTOs.Transaction;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITransactionService
{
    GetTransactionDto GetTransactionById(Guid transactionId);

    /// <summary>
    /// SELECT * FROM Transactions;
    /// </summary>
    /// <returns></returns>
    Task<List<GetTransactionDto>> GetTransactions();

    Task InsertTransaction(InsertTransactionDto transaction);

    Task UpdateTransaction(UpdateTransactionDto transaction);

    void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
