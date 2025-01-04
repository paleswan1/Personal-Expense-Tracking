using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.DTOs.Debts;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Filters.Debts;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class DebtService(IGenericRepository genericRepository, IUserService userService) : IDebtService
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
    
    public async Task<decimal> GetPendingDebtAmounts()
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts = debts.Where(x => x.CreatedBy == userDetails.Id).ToList();

        return debts.Where(x => x.Status == DebtStatus.Pending).Sum(x => x.Amount);
    }

    public async Task<GetDebtsCountDto> GetDebtsCount()
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts = debts.Where(x => x.CreatedBy == userDetails.Id).ToList();

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
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debt = debts.FirstOrDefault(x => x.Id == id);

        if (debt == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        return new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
            ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Status = debt.Status != DebtStatus.Cleared 
                ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now) 
                    ? DebtStatus.PastDue 
                    : DebtStatus.Pending 
                : DebtStatus.Cleared
        };
    }

    public async Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest)
    {
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);
        
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        debts = debts.Where(x => x.CreatedBy == userDetails.Id).ToList();

        if (!string.IsNullOrEmpty(debtFilterRequest.Search))
        {
            debts = debts.Where(x => x.Title.Contains(debtFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (debtFilterRequest.Status != null)
        {
            if (debtFilterRequest.Status == DebtStatus.Pending)
            {
                debts = debts.Where(x => x.Status != DebtStatus.Cleared && x.DueDate >= DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else if (debtFilterRequest.Status == DebtStatus.PastDue)
            {
                debts = debts.Where(x => x.Status != DebtStatus.Cleared && x.DueDate < DateOnly.FromDateTime(DateTime.Now)).ToList();
            }
            else if (debtFilterRequest.Status == DebtStatus.Cleared)
            {
                debts = debts.Where(x => x.Status == DebtStatus.Cleared).ToList();
            }
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
        
        return debts.Select(debt => new GetDebtDto
        {
            Id = debt.Id,
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate.ToString("dd.MM.yyyy"),
            ClearedDate = debt.ClearedDate?.ToString("dd.MM.yyyy hh:mm:ss tt"),
            Status = debt.Status != DebtStatus.Cleared 
                ? debt.DueDate < DateOnly.FromDateTime(DateTime.Now) 
                    ? DebtStatus.PastDue 
                    : DebtStatus.Pending 
                : DebtStatus.Cleared
        }).ToList();
    }

    public async Task InsertDebt(InsertDebtDto debt)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debtModel = new Debt
        {
            Title = debt.Title,
            Source = debt.Source,
            Amount = debt.Amount,
            DueDate = debt.DueDate != null ? DateOnly.FromDateTime(debt.DueDate.Value) : DateOnly.FromDateTime(DateTime.Now),
            Status = DebtStatus.Pending,
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        debts.Add(debtModel);

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task UpdateDebt(UpdateDebtDto debt)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.Title = debt.Title;
        debtModel.Source = debt.Source;
        debtModel.Amount = debt.Amount;
        debtModel.DueDate = debt.DueDate != null
            ? DateOnly.FromDateTime(debt.DueDate.Value)
            : DateOnly.FromDateTime(DateTime.Now);
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task ClearDebt(Guid debtId)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debtId);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        if (debtModel.Status == DebtStatus.Cleared)
        {
            throw new Exception("You can not clear an already cleared debt.");
        }

        var balance = await GetRemainingBalance();

        if (balance < debtModel.Amount)
        {
            throw new Exception("You do not have sufficient balance to clear the following debt, please add sources of incoming transactions to clear the respective debt.");
        } 
        
        debtModel.Status = DebtStatus.Cleared;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;
        debtModel.ClearedDate = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }

    public async Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt)
    {
        var userDetails = await userService.GetUserDetails();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>(Constants.FilePath.AppDebtsDirectoryPath);

        var debtModel = debts.FirstOrDefault(x => x.Id == debt.Id);

        if (debtModel == null)
        {
            throw new Exception("A debt with the following identifier couldn't be found.");
        }

        debtModel.IsActive = false;
        debtModel.LastModifiedBy = userDetails.Id;
        debtModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(debts, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppDebtsDirectoryPath);
    }
}
