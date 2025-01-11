using ExpenseTracker.Domain.Common.Enum;

namespace ExpenseTracker.Application.DTOs.Filters.Debts;

public class GetDebtFilterRequestDto : GetFilterRequestDto
{
    public DebtStatus? Status { get; set; }
    
    public DateTime? StartDate { get; set; } 

    public DateTime? EndDate { get; set; } 
}