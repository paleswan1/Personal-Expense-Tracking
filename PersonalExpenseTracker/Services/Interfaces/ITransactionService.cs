using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Filters.Transactions;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ITransactionService
{
    decimal GetRemainingBalance();
    
    GetTransactionsCountDto GetTransactionsCount();

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
