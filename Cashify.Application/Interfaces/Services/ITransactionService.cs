using Cashify.Application.DTOs.Transactions;
using Cashify.Application.Interfaces.Dependency;
using Cashify.Application.DTOs.Filters.Transactions;

namespace Cashify.Application.Interfaces.Services;

public interface ITransactionService : ITransientService
{
    Task<decimal> GetRemainingBalance();
    
    Task<GetTransactionsCountDto> GetTransactionsCount();

    GetTransactionDto GetTransactionById(Guid transactionId);

    Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task InsertTransaction(InsertTransactionDto transaction);

    Task UpdateTransaction(UpdateTransactionDto transaction);

    Task ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction);

    Task ExportTransactionDetailsToCsv(GetTransactionFilterRequestDto transactionFilterRequest);
}
