using ExpenseTracker.Domain.Models;
using ExpenseTracker.Domain.Common.Enum;
using ExpenseTracker.Application.DTOs.Debts;
using ExpenseTracker.Application.DTOs.Sources;
using ExpenseTracker.Application.Interfaces.Utility;
using ExpenseTracker.Application.DTOs.Filters.Debts;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Interfaces.Repository;

namespace ExpenseTracker.Infrastructure.Implementations.Services;

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
        
        var debts = genericRepository.GetAll<Debt>();

        var pendingDebts = debts.Where(x => x.CreatedBy == userIdentifier).ToList();

        return pendingDebts.Where(x => x.Status is not DebtStatus.Cleared).Sum(x => x.Amount);
    }

    public async Task<GetDebtsCountDto> GetDebtsCount()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>();

        debts = debts.Where(x => x.CreatedBy == userIdentifier).ToList();

        return new GetDebtsCountDto
        {
            All = debts.Count,
            Cleared = debts.Count(x => x.Status == DebtStatus.Cleared),
            Pending = debts.Count(x => x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now)),
            PastDue = debts.Count(x => x.Status != DebtStatus.Cleared && x.DueDate <= DateOnly.FromDateTime(DateTime.Now))
        };
    }
    
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

    public async Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest)
    {
        var debts = genericRepository.GetAll<Debt>();
        
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        debts = debts.Where(x => x.CreatedBy == userIdentifier).ToList();

        if (!string.IsNullOrEmpty(debtFilterRequest.Search))
        {
            debts = debts.Where(x => x.Title.Contains(debtFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

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
        
        if (debtFilterRequest.StartDate != null)
        {
            debts = debts.Where(x => x.DueDate >= DateOnly.FromDateTime(debtFilterRequest.StartDate.Value)).ToList();
        }

        if (debtFilterRequest.EndDate != null)
        {
            debts = debts.Where(x => x.DueDate < DateOnly.FromDateTime(debtFilterRequest.EndDate.Value)).ToList();
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
        
        var result = new List<GetDebtDto>();

        foreach (var debt in debts)
        {
            var source = genericRepository.GetFirstOrDefault<DebtSource>(x => x.Id == debt.SourceId)
                         ?? throw new Exception("A source with the following identifier couldn't be found.");
            
            var debtModel = new GetDebtDto
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
            };

            result.Add(debtModel);
        }

        return result;
    }

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
