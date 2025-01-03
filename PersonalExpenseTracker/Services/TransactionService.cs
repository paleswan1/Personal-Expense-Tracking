using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService, ITagService tagService) : ITransactionService
{
    private static string appDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
    private static string appTransactionsFilePath = ExtensionMethods.GetAppTransactionsFilePath();
    private static string appTransactionTagsFilePath = ExtensionMethods.GetAppTransactionTagsFilePath();

    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transaction = transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>(appTransactionTagsFilePath);

        transactionTags = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

        var tags = new List<GetTagDto>();
        
        foreach(var transactionTag in transactionTags)
        {
            var tag = tagService.GetTagById(transactionTag.TagId);

            tags.Add(tag);
        }

        return new GetTransactionDto
        {
            Id = transaction.Id,
            Title = transaction.Title,
            Note = transaction.Note,
            Type = transaction.Type,
            Source = transaction.Source,
            Amount = transaction.Amount,
            Tags = tags,
        }; 
    }

    public List<GetTransactionDto> GetTransactions()
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(appTransactionTagsFilePath);

        var userDetails = userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        var result = new List<GetTransactionDto>();

        foreach (var transaction in transactions)
        {
            var transactionTagsModel = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

            var tags = new List<GetTagDto>();
        
            foreach(var transactionTag in transactionTags)
            {
                var tag = tagService.GetTagById(transactionTag.TagId);

                tags.Add(tag);
            }

            result.Add(new GetTransactionDto
            {
                Id = transaction.Id,
                Title = transaction.Title,
                Note = transaction.Note,
                Type = transaction.Type,
                Source = transaction.Source,
                Amount = transaction.Amount,
                Tags = tags
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

        if (transaction.Type == Models.Constant.TransactionType.Outflows)
        {
            var incomingCashFlows = genericRepository.GetAll<Transaction>(appTransactionsFilePath);
        }

        var transactionModel = new Transaction
        {
            Id = Guid.NewGuid(),
            Title = transaction.Title,
            Note = transaction.Note,
            Type = transaction.Type,
            Source = transaction.Source,
            Amount = transaction.Amount,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        transactions.Add(transactionModel);

        genericRepository.SaveAll(transactions, appDataDirectoryPath, appTransactionsFilePath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(appTransactionTagsFilePath);

        foreach(var tagId in transaction.TagIds)
        {
            var tag = tagService.GetTagById(tagId);

            var transactionTag = new TransactionTags
            {
                CreatedBy = userDetails.Id,
                CreatedAt = DateTime.Now,
                TransactionId = transactionModel.Id,
                TagId = tag.Id,
            };

            transactionTags.Add(transactionTag);
        }

        genericRepository.SaveAll(transactionTags, appDataDirectoryPath, appTransactionTagsFilePath);
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

        var transactionTags = genericRepository.GetAll<TransactionTags>(appTransactionTagsFilePath);

        transactionTags.RemoveAll(x => x.TransactionId == transactionModel.Id);
    
        foreach(var tagId in transaction.TagIds)
        {
            var tag = tagService.GetTagById(tagId);

            var transactionTag = new TransactionTags
            {
                CreatedBy = userDetails.Id,
                CreatedAt = DateTime.Now,
                TransactionId = transactionModel.Id,
                TagId = tag.Id,
            };

            transactionTags.Add(transactionTag);
        }

        genericRepository.SaveAll(transactionTags, appDataDirectoryPath, appTransactionTagsFilePath);
    }

    public void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        var transactions = genericRepository.GetAll<Transaction>(appTransactionsFilePath);

        var transactionModel = transactions.FirstOrDefault(x => x.Id == transaction.Id);

        if (transactionModel == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>(appTransactionTagsFilePath);

        transactionTags.RemoveAll(x => x.TransactionId == transactionModel.Id);

        genericRepository.SaveAll(transactionTags, appDataDirectoryPath, appTransactionTagsFilePath);

        transactions.Remove(transactionModel);

        genericRepository.SaveAll(transactions, appDataDirectoryPath, appTransactionsFilePath);
    }
    
}
