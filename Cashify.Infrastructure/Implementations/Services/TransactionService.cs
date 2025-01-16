using Cashify.Domain.Models;
using Cashify.Domain.Common.Enum;
using Cashify.Domain.Common.Constants;
using Cashify.Application.DTOs.Transactions;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Managers;
using Cashify.Application.Interfaces.Repository;
using Cashify.Application.DTOs.Filters.Transactions;

namespace Cashify.Infrastructure.Implementations.Services;

/// <summary>
/// Provides services for managing transactions, including retrieval, creation, updating, and activation/deactivation.
/// </summary>
/// <param name="genericRepository">Generic repository for accessing data</param>
/// <param name="userService">Service for managing user-related operations.</param>
/// <param name="tagService"> Service for managing tags associated with transactions.</param>
public class TransactionService(IGenericRepository genericRepository, IUserService userService, ITagService tagService, ICsvManager csvManager) : ITransactionService
{
    /// <summary>
    /// Retrieves the remaining balance of the current user by calculating inflows, outflows, and cleared debts. 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception">The remaining balance as a decimal value.</exception>
    public async Task<decimal> GetRemainingBalance()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>(x => x.CreatedBy == userIdentifier);

        var clearedDebts = genericRepository.GetAll<Debt>(x => x.CreatedBy == userIdentifier && x.Status == DebtStatus.Cleared);

        return transactions.Where(x => x.Type == TransactionType.Inflow).Sum(x => x.Amount) -
               transactions.Where(x => x.Type == TransactionType.Outflow).Sum(x => x.Amount) -
               clearedDebts.Sum(x => x.Amount);
    }

    /// <summary>
    /// Gets the count of all transactions, inflows, and outflows for the current user.
    /// </summary>
    /// <returns>A DTO containing counts of different transaction types.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<GetTransactionsCountDto> GetTransactionsCount()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>(x => x.CreatedBy == userIdentifier);

        return new GetTransactionsCountDto
        {
            AllCount = transactions.Count,
            InflowsCount = transactions.Count(x => x.Type == TransactionType.Inflow),
            OutflowsCount = transactions.Count(x => x.Type == TransactionType.Outflow)
        };
    }

    /// <summary>
    ///  Retrieves a specific transaction by its identifier.
    /// </summary>
    /// <param name="transactionId">The unique identifier of the transaction</param>
    /// <returns>Details of the specified transaction</returns>
    /// <exception cref="Exception"></exception>
    public GetTransactionDto GetTransactionById(Guid transactionId)
    {
        var transactions = genericRepository.GetAll<Transaction>();

        var transaction = transactions.FirstOrDefault(x => x.Id == transactionId);

        if (transaction == null)
        {
            throw new Exception("A transaction with the following identifier couldn't be found.");
        }

        var transactionTags = genericRepository.GetAll<TransactionTags>(x => x.TransactionId == transaction.Id);

        var tags = transactionTags.Select(x => tagService.GetTagById(x.TagId)).ToList();

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

    /// <summary>
    /// Retrieves all transactions with optional filters such as date range, tags, and transaction type.
    /// </summary>
    /// <param name="transactionFilterRequest">Filtering criteria for transactions.</param>
    /// <returns>List of transactions matching the specified filters.</returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<GetTransactionDto>> GetAllTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {

        var transactionTags = genericRepository.GetAll<TransactionTags>();

        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var transactions = genericRepository.GetAll<Transaction>(x => x.CreatedBy == userIdentifier
            && (string.IsNullOrEmpty(transactionFilterRequest.Search) || x.Title.Contains(transactionFilterRequest.Search, StringComparison.OrdinalIgnoreCase))
            && (transactionFilterRequest.TransactionType == null || x.Type == transactionFilterRequest.TransactionType)
            && (transactionFilterRequest.StartDate == null || x.CreatedDate >= transactionFilterRequest.StartDate)
            && (transactionFilterRequest.EndDate == null || x.CreatedDate <= transactionFilterRequest.EndDate));
        
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

        return (from transaction in transactions
            let transactionTagsModel = transactionTags.Where(x => x.TransactionId == transaction.Id).ToList()
            where transactionTagsModel.Select(x => x.TagId).Any(tagId => transactionFilterRequest.TagIds.Count == 0 || transactionFilterRequest.TagIds.Contains(tagId))
            let tags = transactionTagsModel.Select(transactionTag => tagService.GetTagById(transactionTag.TagId)).ToList()
            select new GetTransactionDto
            {
                Id = transaction.Id,
                Title = transaction.Title,
                Note = transaction.Note,
                Type = transaction.Type,
                Source = transaction.Source,
                Amount = transaction.Amount,
                Date = transaction.CreatedDate.ToString("dd.MM.yyyy hh:mm:ss tt"),
                Tags = tags
            }).ToList();
    }

    /// <summary>
    /// Inserts a new transaction into the system, validating balance for outflows and adding associated tags.
    /// </summary>
    /// <param name="transaction">The transaction to be inserted.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Updates an existing transaction, including its details and associated tags.
    /// </summary>
    /// <param name="transaction">The updated transaction details.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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

    /// <summary>
    /// Toggles the active status of a transaction (activates or deactivates).
    /// </summary>
    /// <param name="transaction">The transaction whose active status is to be toggled.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task ActivateDeactivateTransaction(ActivateDeactivateTransactionDto transaction)
    {
        var transactionModel = genericRepository.GetFirstOrDefault<Transaction>(x => x.Id == transaction.Id)
                               ?? throw new Exception("A transaction with the following identifier couldn't be found.");
        
        transactionModel.IsActive = !transactionModel.IsActive;
        
        await genericRepository.Update(transactionModel);
    }

    public async Task ExportTransactionDetailsToCsv(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var transactions = await GetAllTransactions(transactionFilterRequest);

        var userIdentifier = await userService.GetUserId();
        
        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var result = transactions.Select(x => new
        {
            Id = x.Id,
            Title = x.Title,
            Amount = x.Amount,
            Date = x.Date,
            Note = x.Note,
            Type = x.Type.ToString(),
            Source = x.Source.ToString(),
            Tags = $"Tags - ({string.Join(", ", x.Tags.Select(y => y.Title))})"
        }).ToList();
        
        var transactionDetails = csvManager.GenerateCsv(result);
        
        await csvManager.SaveCsvFileAsync(Constants.ModelPath.TransactionDetails, transactionDetails);
    }
}