using ExpenseTracker.Domain.Common.Base;

namespace ExpenseTracker.Domain.Models;

public class DebtSource : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public string BackgroundColor { get; set; } = string.Empty;
    
    public string TextColor { get; set; } = string.Empty;
}