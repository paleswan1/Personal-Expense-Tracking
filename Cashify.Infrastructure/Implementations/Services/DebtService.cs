using Cashify.Domain.Models;
using Cashify.Domain.Common.Enum;
using Cashify.Application.DTOs.Debts;
using Cashify.Application.DTOs.Sources;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.Interfaces.Repository;

namespace Cashify.Infrastructure.Implementations.Services;

public class DebtService(IGenericRepository genericRepository, 
    IUserService userService,
    ITransactionService transactionService) : IDebtService
{
    public async Task<decimal> GetPendingDebtAmounts()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var pendingDebts = genericRepository.GetAll<Debt>(x => x.CreatedBy == userIdentifier && x.Status is not DebtStatus.Cleared);

        return pendingDebts.Sum(x => x.Amount);
    }

    /// <summary>
    /// Retrieves the count of all debts, cleared debts, pending debts, and past-due debts for the logged-in user.
    /// </summary>
    /// <returns>object containing the counts.</returns>
    /// <exception cref="Exception">Thrown if the user is not logged in.</exception>
    public async Task<GetDebtsCountDto> GetDebtsCount()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>(x => x.CreatedBy == userIdentifier);

        return new GetDebtsCountDto
        {
            All = debts.Count,
            Cleared = debts.Count(x => x.Status == DebtStatus.Cleared),
            Pending = debts.Count(x => x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now)),
            PastDue = debts.Count(x => x.Status != DebtStatus.Cleared && x.DueDate <= DateOnly.FromDateTime(DateTime.Now))
        };
    }

    /// <summary>
    /// Retrieves the details of a specific debt by its ID.
    /// </summary>
    /// <param name="id">The ID of the debt to retrieve.</param>
    /// <returns>Details of the debt.</returns>
    /// <exception cref="Exception">Thrown if the debt or its source cannot be found.</exception>
    public GetDebtDto GetDebtById(Guid id)
    {
        var debts = genericRepository.GetAll<Debt>();

        var debt = debts.FirstOrDefault(x => x.Id == id)
            ?? throw new Exception("A debt with the following identifier couldn't be found.");

        var source = genericRepository.GetFirstOrDefault<DebtSource>(x => x.Id == debt.SourceId)
            ?? throw new Exception("A source with the following identifier couldn't be found.");
        
        return new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = new GetSourceDto()
            {
                Id = source.Id,
                Title = source.Title,
                BackgroundColor = source.BackgroundColor,
                TextColor = source.TextColor
            },
            Amount = debt.Amount,
            DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
            ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Status = debt.Status != DebtStatus.Cleared 
                ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now) 
                    ? DebtStatus.Overdue 
                    : DebtStatus.Pending 
                : DebtStatus.Cleared
        };
    }

    /// <summary>
    /// Retrieves a list of debts based on the specified filters. 
    /// </summary>
    /// <param name="debtFilterRequest">An object containing the filter criteria.</param>
    /// <returns> objects matching the filters.</returns>
    /// <exception cref="Exception">Thrown if the user is not logged in.</exception>
    public async Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(x => 
            x.CreatedBy == userIdentifier 
            && (debtFilterRequest.StartDate == null || x.DueDate >= DateOnly.FromDateTime(debtFilterRequest.StartDate.Value))
            && (debtFilterRequest.EndDate == null || x.DueDate <= DateOnly.FromDateTime(debtFilterRequest.EndDate.Value))
            && (string.IsNullOrEmpty(debtFilterRequest.Search) || x.Title.Contains(debtFilterRequest.Search, StringComparison.OrdinalIgnoreCase)));

        if (debtFilterRequest.Status != null)
        {
            debts = debtFilterRequest.Status switch
            {
                DebtStatus.Pending => debts.Where(x =>
                        x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now))
                    .ToList(),
                DebtStatus.Overdue => debts.Where(x =>
                        x.Status != DebtStatus.Cleared && x.DueDate < DateOnly.FromDateTime(DateTime.Now))
                    .ToList(),
                DebtStatus.Cleared => debts.Where(x => x.Status == DebtStatus.Cleared).ToList(),
                _ => debts
            };
        }
        
        if (!string.IsNullOrEmpty(debtFilterRequest.OrderBy))
        {
            debts = debtFilterRequest.OrderBy switch
            {
                "Title" => debtFilterRequest.IsDescending ? debts.OrderByDescending(x => x.Title).ToList() : debts.OrderBy(x => x.Title).ToList(),
                "Amount" => debtFilterRequest.IsDescending ? debts.OrderByDescending(x => x.Amount).ToList() : debts.OrderBy(x => x.Amount).ToList(),
                "DueDate" => debtFilterRequest.IsDescending ? debts.OrderByDescending(x => x.DueDate).ToList() : debts.OrderBy(x => x.DueDate).ToList(),
                "ClearedDate" => debtFilterRequest.IsDescending ? debts.OrderByDescending(x => x.ClearedDate).ToList() : debts.OrderBy(x => x.ClearedDate).ToList(),
                _ => debts
            };
        }

        return (from debt in debts
            let source = genericRepository.GetFirstOrDefault<DebtSource>(x => 
                x.Id == debt.SourceId) 
                         ?? throw new Exception("A source with the following identifier couldn't be found.")
            select new GetDebtDto
            {
                Id = debt.Id,
                Title = debt.Title,
                Source = new GetSourceDto
                {
                    Id = source.Id, 
                    Title = source.Title, 
                    BackgroundColor = source.BackgroundColor, 
                    TextColor = source.TextColor
                },
                Amount = debt.Amount,
                DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
                ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
                Status = debt.Status != DebtStatus.Cleared
                    ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now)
                        ? DebtStatus.Overdue
                        : DebtStatus.Pending
                    : DebtStatus.Cleared
            }).ToList();
    }

    /// <summary>
    /// Inserting a new debt record into the database.
    /// </summary>
    /// <param name="debt">An object containing details of the debt to be inserted.</param>
    /// <returns></returns>
    /// <exception cref="Exception">Thrown if the user is not logged in.</exception>
    public async Task InsertDebt(InsertDebtDto debt)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = new Debt
        {
            Title = debt.Title,
            SourceId = debt.SourceId,
            Amount = debt.Amount,
            DueDate = debt.DueDate != null 
                ? DateOnly.FromDateTime(debt.DueDate.Value) 
                : DateOnly.FromDateTime(DateTime.Now),
            Status = DebtStatus.Pending
        };

        await genericRepository.Insert(debtModel);
    }

    /// <summary>
    /// Updates the details of an existing debt.
    /// </summary>
    /// <param name="debt">An object containing updated details of the debt.</param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the user is not logged in or if the debt with the specified ID cannot be found.
    /// </exception>
    public async Task UpdateDebt(UpdateDebtDto debt)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = genericRepository.GetFirstOrDefault<Debt>(x => x.Id == debt.Id)
            ?? throw new Exception("A debt with the following identifier couldn't be found.");

        debtModel.Title = debt.Title;
        debtModel.SourceId = debt.SourceId;
        debtModel.Amount = debt.Amount;
        debtModel.DueDate = debt.DueDate != null
            ? DateOnly.FromDateTime(debt.DueDate.Value)
            : DateOnly.FromDateTime(DateTime.Now);

        await genericRepository.Update(debtModel);
    }

    /// <summary>
    /// Marks a specified debt as cleared.
    /// </summary>
    /// <param name="debtId">The ID of the debt to be cleared</param>
    public async Task ClearDebt(Guid debtId)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = genericRepository.GetFirstOrDefault<Debt>(x => x.Id == debtId)
                        ?? throw new Exception("A debt with the following identifier couldn't be found.");

        if (debtModel.Status == DebtStatus.Cleared)
        {
            throw new Exception("You can not clear an already cleared debt.");
        }

        var balance = await transactionService.GetRemainingBalance();

        if (balance < debtModel.Amount)
        {
            throw new Exception("You do not have sufficient balance to clear the following debt, please add sources of incoming transactions to clear the respective debt.");
        } 
        
        debtModel.Status = DebtStatus.Cleared;
        debtModel.ClearedDate = DateTime.Now;

        await genericRepository.Update(debtModel);
    }

    /// <summary>
    /// Activates or deactivates a specified debt.
    /// </summary>
    /// <param name="debt"> An object containing the ID of the debt to be activated or deactivated.</param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the user is not logged in or if the debt with the specified ID cannot be found.
    /// </exception>
    public async Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debtModel = genericRepository.GetFirstOrDefault<Debt>(x => x.Id == debt.Id)
                        ?? throw new Exception("A debt with the following identifier couldn't be found.");

        debtModel.IsActive = false;

        await genericRepository.Update(debtModel);
    }
}
