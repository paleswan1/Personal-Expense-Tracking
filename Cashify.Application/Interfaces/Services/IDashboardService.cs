using Cashify.Application.DTOs.Debts;
using Cashify.Application.DTOs.Dashboard;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.Interfaces.Dependency;
using Cashify.Application.DTOs.Filters.Dashboard;

namespace Cashify.Application.Interfaces.Services;

public interface IDashboardService : ITransientService
{
    Task<GetDashboardCount> GetDashboardCount();

    Task<List<GetDebtDto>> GetPendingDebts(GetDebtFilterRequestDto debtFilterRequest);

    Task<List<GetTransactionDetails>> GetInflowsTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task<List<GetTransactionDetails>> GetOutflowsTransactions(GetTransactionFilterRequestDto transactionFilterRequest);

    Task<List<GetTransactionDetails>> GetDebtsTransactions(GetTransactionFilterRequestDto transactionFilterRequest);
}