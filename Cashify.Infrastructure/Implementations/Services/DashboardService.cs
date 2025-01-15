using Cashify.Domain.Models;
using Cashify.Domain.Common.Enum;
using Cashify.Application.DTOs.Debts;
using Cashify.Application.DTOs.Dashboard;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.Interfaces.Services;
using Cashify.Application.DTOs.Filters.Dashboard;
using Cashify.Application.Interfaces.Repository;

namespace Cashify.Infrastructure.Implementations.Services;

public class DashboardService(IGenericRepository genericRepository, IDebtService debtService, IUserService userService) : IDashboardService
{
    // Retrieves dashboard summary counts for debts and transactions
    public async Task<GetDashboardCount> GetDashboardCount()
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }

        var debts = genericRepository.GetAll<Debt>().Where(x => x.CreatedBy == userIdentifier).ToList();
        
        var transactions = genericRepository.GetAll<Transaction>().Where(x => x.CreatedBy == userIdentifier).ToList();
        
        return new GetDashboardCount
        {
            TotalDebtsAmount = debts.Sum(x => x.Amount),
            TotalDebtsCount = debts.Count,
            TotalInflowsAmount = transactions.Where(x => x.Type == TransactionType.Inflow).Sum(x => x.Amount),
            TotalInflowsCount = transactions.Count(x => x.Type == TransactionType.Inflow),
            TotalOutflowsAmount = transactions.Where(x => x.Type == TransactionType.Outflow).Sum(x => x.Amount),
            TotalOutflowsCount = transactions.Count(x => x.Type == TransactionType.Outflow),
            TotalClearedDebtsAmount = debts.Where(x => x.Status == DebtStatus.Cleared).Sum(x => x.Amount),
            TotalClearedDebtsCount = debts.Count(x => x.Status == DebtStatus.Cleared),
            TotalPendingDebtsAmount = debts.Where(x => x.Status != DebtStatus.Cleared).Sum(x => x.Amount),
            TotalPendingDebtsCount = debts.Count(x => x.Status != DebtStatus.Cleared),
        };
    }

    // Retrieves a combined list of pending and overdue debts
    public async Task<List<GetDebtDto>> GetPendingDebts(GetDebtFilterRequestDto debtFilterRequest)
    {
        var pendingFilterRequest = new GetDebtFilterRequestDto()
        {
            Status = DebtStatus.Pending,
            Search = debtFilterRequest.Search,
            OrderBy = debtFilterRequest.OrderBy,
            EndDate = debtFilterRequest.EndDate,
            StartDate = debtFilterRequest.StartDate,
            IsDescending = debtFilterRequest.IsDescending
        };
        
        var pendingDebts =  await debtService.GetAllDebts(pendingFilterRequest);
        
        var overDueFilterRequest = new GetDebtFilterRequestDto()
        {
            Status = DebtStatus.Overdue,
            Search = debtFilterRequest.Search,
            OrderBy = debtFilterRequest.OrderBy,
            EndDate = debtFilterRequest.EndDate,
            StartDate = debtFilterRequest.StartDate,
            IsDescending = debtFilterRequest.IsDescending
        };
        
        var overDueDebts =  await debtService.GetAllDebts(overDueFilterRequest);

        return pendingDebts.Concat(overDueDebts).ToList();
    }

    // Retrieves inflow transactions based on filter criteria
    public async Task<List<GetTransactionDetails>> GetInflowsTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>().Where(x => 
            x.Type == TransactionType.Inflow && x.CreatedBy == userIdentifier).ToList();

        // Order transactions and return the result
        var result = transactionFilterRequest.IsAscending 
            ? transactions.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
            : transactions.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

        return result.Select(x => new GetTransactionDetails
        {
            Title = x.Title,
            Amount = x.Amount
        }).ToList();
    }

    // Retrieves outflow transactions based on filter criteria
    public async Task<List<GetTransactionDetails>> GetOutflowsTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var transactions = genericRepository.GetAll<Transaction>().Where(x => 
            x.Type == TransactionType.Outflow && x.CreatedBy == userIdentifier).ToList();

        var result = transactionFilterRequest.IsAscending 
            ? transactions.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
            : transactions.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

        return result.Select(x => new GetTransactionDetails
        {
            Title = x.Title,
            Amount = x.Amount
        }).ToList();
    }

    // Retrieves debt transactions based on filter criteria
    public async Task<List<GetTransactionDetails>> GetDebtsTransactions(GetTransactionFilterRequestDto transactionFilterRequest)
    {
        var userIdentifier = await userService.GetUserId();

        if (userIdentifier == Guid.Empty)
        {
            throw new Exception("You are not logged in.");
        }
        
        var debts = genericRepository.GetAll<Debt>().Where(x => 
            x.CreatedBy == userIdentifier).ToList();

        var result = transactionFilterRequest.IsAscending 
            ? debts.OrderBy(x => x.Amount).Take(transactionFilterRequest.Count).ToList() 
            : debts.OrderByDescending(x => x.Amount).Take(transactionFilterRequest.Count).ToList();

        return result.Select(x => new GetTransactionDetails
        {
            Title = x.Title,
            Amount = x.Amount
        }).ToList();
    }
}