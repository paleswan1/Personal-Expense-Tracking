using ExpenseTracker.Application.DTOs.Transactions;
using ExpenseTracker.Application.Interfaces.Dependency;
using ExpenseTracker.Application.DTOs.Filters.Transactions;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface ITransactionService : ITransientService
{
    Task<decimal> GetRemainingBalance();
    
    Task<GetTransactionsCountDto> GetTransactionsCount();

    GetTransactionDto GetTransactionById(Guid transactionId);

    Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task InsertTransaction(InsertTransactionDto transaction);

    Task UpdateTransaction(UpdateTransactionDto transaction);

    Task ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);
}
