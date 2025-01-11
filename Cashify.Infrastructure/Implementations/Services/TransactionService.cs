using Cashify.Domain.Models;
using Cashify.Domain.Common.Enum;
using Cashify.Application.DTOs.Transactions;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Repository;
using Cashify.Application.DTOs.Filters.Transactions;
using IUserService = Cashify.Application.Interfaces.Utility.IUserService;

namespace Cashify.Infrastructure.Implementations.Services;

public class TransactionService(IGenericRepository genericRepository, IUserService userService, ITagService tagService) : ITransactionService
{
    public async Task<decimal> GetRemainingBalance()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>();

        transactions = transactions.Where(x => x.CreatedBy == userIdentifier).ToList();

        var debts = genericRepository.GetAll<Debt>();

        debts = debts.Where(x => x.CreatedBy == userIdentifier).ToList();

        return transactions.Where(x => x.Type == TransactionType.Inflow).Sum(x => x.Amount) -
               transactions.Where(x => x.Type == TransactionType.Outflow).Sum(x => x.Amount) -
               debts.Where(x => x.Status == DebtStatus.Cleared).Sum(x => x.Amount);
    }
    
    public async Task<GetTransactionsCountDto> GetTransactionsCount()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>();

        transactions = transactions.Where(x => x.CreatedBy == userIdentifier).ToList();

        return new GetTransactionsCountDto
        {
            AllCount = transactions.Count,
            InflowsCount = transactions.Count(x => x.Type == TransactionType.Inflow),
            OutflowsCount = transactions.Count(x => x.Type == TransactionType.Outflow)
        };
    }
    
    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        var transactions = genericRepository.GetAll<Transaction>();

        var transaction = transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>();

        transactionTags = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

        var tags = transactionTags.Select(transactionTag => tagService.GetTagById(transactionTag.TagId)).ToList();

        return new GetTransactionDto
        {
            Id = transaction.Id,
            Title = transaction.Title,
            Note = transaction.Note,
            Type = transaction.Type,
            Source = transaction.Source,
            Amount = transaction.Amount,
            Date = transaction.CreatedDate.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Tags = tags
        }; 
    }

    public async Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var transactions = genericRepository.GetAll<Transaction>();

        var transactionTags = genericRepository.GetAll<TransactionTags>();

        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        transactions = transactions.Where(x => x.CreatedBy == userIdentifier).ToList();

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
            transactions = transactions.Where(x => x.CreatedDate >= transactionFilterRequest.StartDate).ToList();
        }

        if (transactionFilterRequest.EndDate != null)
        {
            transactions = transactions.Where(x => x.CreatedDate <= transactionFilterRequest.EndDate).ToList();
        }
        
        if (!string.IsNullOrEmpty(transactionFilterRequest.OrderBy))
        {
            transactions = transactionFilterRequest.OrderBy switch
            {
                "Title" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.Title).ToList() : transactions.OrderBy(x => x.Title).ToList(),
                "Amount" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.Amount).ToList() : transactions.OrderBy(x => x.Amount).ToList(),
                "Date" => transactionFilterRequest.IsDescending ? transactions.OrderByDescending(x => x.CreatedDate).ToList() : transactions.OrderBy(x => x.CreatedDate).ToList(),
                _ => transactions
            };
        }
        
        var result = new List<GetTransactionDto>();

        foreach (var transaction in transactions)
        {
            var transactionTagsModel = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList();

            if (transactionTagsModel.Select(x => x.TagId).Any(tagId => transactionFilterRequest.TagIds.Count == 0 || transactionFilterRequest.TagIds.Contains(tagId)))
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
                    Date = transaction.CreatedDate.ToString("dd.MM.yyyy hh:mm:ss tt"),
                    Tags = tags
                });
            }
        }

        return result;
    }

    public async Task InsertTransaction(InsertTransactionDto transaction)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        if (transaction.Type == TransactionType.Outflow)
        {
            var remainingBalance = await GetRemainingBalance();
            
            if (transaction.Amount > remainingBalance)
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
            Amount = transaction.Amount
        };

        var transactions = genericRepository.GetAll<Transaction>();

        transactions.Add(transactionModel);

        await genericRepository.Insert(transactionModel);

        var transactionTags = transaction.TagIds
            .Select(tagService.GetTagById)
            .Select(tag => new TransactionTags
            {
                TagId = tag.Id, 
                TransactionId = transactionModel.Id
            }).ToList();

        foreach (var transactionTag in transactionTags)
        {
            await genericRepository.Insert(transactionTag);
        }
    }

    public async Task UpdateTransaction(UpdateTransactionDto transaction)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var transactionModel = genericRepository.GetFirstOrDefault<Transaction>(x => x.Id == transaction.Id)
            ?? throw new Exception("A transaction with the following identifier couldn't be found.");

        transactionModel.Amount = transaction.Amount;
        transactionModel.Note = transaction.Note;
        transactionModel.Type = transaction.Type;   
        transactionModel.Source = transaction.Source;   
        transactionModel.Amount = transaction.Amount;

        await genericRepository.Update(transactionModel);

        var transactionTags = genericRepository.GetAll<TransactionTags>();

        var existingTransactionTags = transactionTags.Where(x => x.TransactionId == transactionModel.Id).ToList();
        
        foreach (var existingTransactionTag in existingTransactionTags)
        {
            genericRepository.Delete(existingTransactionTag);
        }

        var transactionTagModels = transaction.TagIds
            .Select(tagService.GetTagById)
            .Select(tag => new TransactionTags
            {
                TagId = tag.Id, 
                TransactionId = transactionModel.Id
            });

        foreach (var transactionTagModel in transactionTagModels)
        {
            await genericRepository.Insert(transactionTagModel);
        }
    }

    public async Task ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        var transactionModel = genericRepository.GetFirstOrDefault<Transaction>(x => x.Id == transaction.Id)
                               ?? throw new Exception("A transaction with the following identifier couldn't be found.");
        
        transactionModel.IsActive = !transactionModel.IsActive;
        
        await genericRepository.Update(transactionModel);
    }
}