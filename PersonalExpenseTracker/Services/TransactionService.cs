using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Services.Interfaces;
using PersonalExpenseTracker.Filters.Transactions;

namespace PersonalExpenseTracker.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService, ITagService tagService) : ITransactionService
{
    public async Task<decimal> GetRemainingBalance()
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts = debts.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return transactions.Where(x => x.Type == TransactionType.Inflows).Sum(x => x.Amount) -
               transactions.Where(x => x.Type == TransactionType.Outflows).Sum(x => x.Amount) -
               debts.Where(x => x.Status == DebtStatus.Cleared).Sum(x => x.Amount);
    }
    
    public async Task<GetTransactionsCountDto> GetTransactionsCount()
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return new GetTransactionsCountDto()
        {
            AllCount = transactions.Count,
            InflowsCount = transactions.Count(x => x.Type == TransactionType.Inflows),
            OutflowsCount = transactions.Count(x => x.Type == TransactionType.Outflows)
        };
    }
    
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
            Date = transaction.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Tags = tags,
        }; 
    }

    public async Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var transactions = genericRepository.GetAll<Transaction>(Constants.FilePath.AppTransactionsDirectoryPath);

        var transactionTags = genericRepository.GetAll<TransactionTags>(Constants.FilePath.AppTransactionTagsDirectoryPath);

        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        transactions = transactions.Where(x => x.CreatedBy == userDetails.Id).ToList();

        if (!string.IsNullOrEmpty(transactionFilterRequest.Search))
        {
            transactions = transactions.Where(x => x.Title.Contains(transactionFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (transactionFilterRequest.TransactionType != null)
        {
            transactions = transactions.Where(x => x.Type == transactionFilterRequest.TransactionType).ToList();
        }
        
        if (transactionFilterRequest.StartDate != null)
        {
            transactions = transactions.Where(x => x.CreatedAt >= transactionFilterRequest.StartDate).ToList();
        }

        if (transactionFilterRequest.EndDate != null)
        {
            transactions = transactions.Where(x => x.CreatedAt <= transactionFilterRequest.EndDate).ToList();
        }
        
        if (!string.IsNullOrEmpty(transactionFilterRequest.OrderBy))
        {
            transactions = transactionFilterRequest.OrderBy switch
            {
                "Title" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.Title).ToList() : transactions.OrderBy(x => x.Title).ToList(),
                "Amount" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.Amount).ToList() : transactions.OrderBy(x => x.Amount).ToList(),
                "Date" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.CreatedAt).ToList() : transactions.OrderBy(x => x.CreatedAt).ToList(),
                _ => transactions
            };
        }
        
        var result = new List<GetTransactionDto>();

        foreach (var transaction in transactions)
        {
            var transactionTagsModel = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

            if (transactionTagsModel.Select(x => x.TagId).Any(tagId => transactionFilterRequest.TagIds.Contains(tagId)) || transactionFilterRequest.TagIds.Count == 0)
            {
                var tags = transactionTagsModel.Select(transactionTag => tagService.GetTagById(transactionTag.TagId)).ToList();

                result.Add(new GetTransactionDto
                {
                    Id = transaction.Id,
                    Title = transaction.Title,
                    Note = transaction.Note,
                    Type = transaction.Type,
                    Source = transaction.Source,
                    Amount = transaction.Amount,
                    Date = transaction.CreatedAt.ToString("dd.MM.yyyy hh:mm:ss tt"),
                    Tags = tags
                });
            }
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