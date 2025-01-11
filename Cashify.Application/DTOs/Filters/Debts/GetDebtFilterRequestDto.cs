using Cashify.Domain.Common.Enum;

namespace Cashify.Application.DTOs.Filters.Debts;

public class GetDebtFilterRequestDto : GetFilterRequestDto
{
    public DebtStatus? Status { get; set; }
    
    public DateTime? StartDate { get; set; } 

    public DateTime? EndDate { get; set; } 
}