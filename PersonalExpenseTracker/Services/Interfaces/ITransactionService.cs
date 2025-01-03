using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Filters.Transactions;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITransactionService
{
    GetTransactionDto GetTransactionById(Guid transactionId);

    /// <summary>
    /// SELECT * FROM Transactions;
    /// </summary>
    /// <returns></returns>
    Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task InsertTransaction(InsertTransactionDto transaction);

    Task UpdateTransaction(UpdateTransactionDto transaction);

    void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
