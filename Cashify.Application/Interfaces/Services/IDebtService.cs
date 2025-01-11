using Cashify.Application.DTOs.Debts;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Services;

public interface IDebtService : ITransientService
{
    Task<decimal> GetPendingDebtAmounts();

    Task<GetDebtsCountDto> GetDebtsCount();
    
    GetDebtDto GetDebtById(Guid id);

    Task<List<GetDebtDto>> GetAllDebts(GetDebtFilterRequestDto debtFilterRequest);

    Task InsertDebt(InsertDebtDto debt);

    Task UpdateDebt(UpdateDebtDto transaction);

    Task ClearDebt(Guid debtId);

    Task ActivateDeactivateDebt(ActivateDeactivateDebtDto debt);
}
