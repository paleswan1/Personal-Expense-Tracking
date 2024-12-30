using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService) : ITransactionService
{
    public void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        throw new NotImplementedException();
    }

    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        throw new NotImplementedException();
    }

    public List<GetTransactionDto> GetTransactions()
    {
        throw new NotImplementedException();
    }

    public void InsertTransaction(InsertTransactionDto transaction)
    {
        throw new NotImplementedException();
    }

    public void UpdateTrasaction(UpdateTransactionDto transaction)
    {
        throw new NotImplementedException();
    }
}
