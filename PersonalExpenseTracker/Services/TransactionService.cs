using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService, ITagService tagService) : ITransactionService
{
    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        var transaction = transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

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

    public async Task<List<GetTransactionDto>> GetTransactions()
    {
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        var result = new List<GetTransactionDto>();

        foreach (var transaction in transactions)
        {
            var transactionTagsModel = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

            var tags = transactionTagsModel.Select(transactionTag => tagService.GetTagById(transactionTag.TagId)).ToList();

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

    public async Task InsertTransaction(InsertTransactionDto transaction)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        if (transaction.Type == TransactionType.Outflows)
        {
            var transactionModels = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);
            var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

            var incomingCashFlowAmounts = transactionModels.Where(x => x.Type == TransactionType.Inflows).Sum(x => x.Amount);
            var clearedDebtAmounts = debts.Where(x => x.Status == DebtStatus.Cleared).Sum(x => x.Amount);

            if (transaction.Amount > incomingCashFlowAmounts + clearedDebtAmounts)
            {
                throw new Exception("You do not have sufficient balance to perform the following cash outflow transaction.");
            }
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

        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        transactions.Add(transactionModel);

        genericRepository.SaveAll(transactions, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionsDirectoryPath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

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

        genericRepository.SaveAll(transactionTags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionTagsDirectoryPath);
    }

    public async Task UpdateTransaction(UpdateTransactionDto transaction)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

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

        genericRepository.SaveAll(transactions, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionsDirectoryPath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

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

        genericRepository.SaveAll(transactionTags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionTagsDirectoryPath);
    }

    public void ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        var transactionModel = transactions.FirstOrDefault(x => x.Id == transaction.Id);

        if (transactionModel == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

        transactionTags.RemoveAll(x => x.TransactionId == transactionModel.Id);

        genericRepository.SaveAll(transactionTags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionTagsDirectoryPath);

        transactions.Remove(transactionModel);

        genericRepository.SaveAll(transactions, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTransactionsDirectoryPath);
    }
}