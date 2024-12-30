using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService) : ITransactionService
{
    private static string appDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
    private static string appTransactionsFilePath = ExtensionMethods.GetAppTagsFilePath();

   
    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transaction = transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        return new GetTransactionDto
        {
            Id = transaction.Id,
            Title = transaction.Title,
            Note = transaction.Note,
            Type = transaction.Type,
            Source = transaction.Source,
            Amount = transaction.Amount,
        };
    }

    public List<GetTransactionDto> GetTransactions()
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        var result = new List<GetTransactionDto>();

        foreach (var transaction in transactions)
        {
            result.Add(new GetTransactionDto
            {
                Id = transaction.Id,
                Title = transaction.Title,
                Note = transaction.Note,
                Type = transaction.Type,
                Source = transaction.Source,
                Amount = transaction.Amount,
            });
        }

        return result;
    }

    public void InsertTransaction(InsertTransactionDto transaction)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var transactionModel = new Transaction
        {
            Title = transaction.Title,
            Note = transaction.Note,
            Type = transaction.Type,
            Source = transaction.Source,
            Amount = transaction.Amount,
        };

        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        transactions.Add(transactionModel);

        genericRepository.SaveAll(transactions, appDataDirectoryPath, appTransactionsFilePath);
    }

    public void UpdateTrasaction(UpdateTransactionDto transaction)
    {
        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transactionModel = transactions.FirstOrDefault(x => x.Id == transaction.Id);

        if (transactionModel == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        transactionModel.Amount = transaction.Amount;
        transactionModel.Note = transaction.Note;
        transactionModel.Type = transaction.Type;   
        transactionModel.Source = transaction.Source;   
        transactionModel.Amount = transaction.Amount;
        transactionModel.LastModifiedBy = userDetails.Id;
        transactionModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(transactions, appDataDirectoryPath, appTransactionsFilePath);
    }

    public void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transactionModel = transactions.FirstOrDefault(x => x.Id == transaction.Id);

        if (transactionModel == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        transactions.Remove(transactionModel);

        genericRepository.SaveAll(transactions, appDataDirectoryPath, appTransactionsFilePath);
    }

}
