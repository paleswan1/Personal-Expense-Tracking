namespace ExpenseTracker.Application.DTOs.Debts;

public class GetDebtsCountDto
{
    public int All { get; set; }
    
    public int Pending { get; set; }
    
    public int Cleared { get; set; }
    
    public int PastDue { get; set; }
}