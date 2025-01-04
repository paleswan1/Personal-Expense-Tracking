using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.Filters.Debts;

public class GetDebtFilterRequestDto : GetFilterRequestDto
{
    public DebtStatus? Status { get; set; }
    
    public DateTime? StartDate { get; set; } 

    public DateTime? EndDate { get; set; } 
}