using ExpenseTracker.Domain.Common.Base;

namespace ExpenseTracker.Domain.Models;

public class Tag : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }

    public bool IsDefault { get; set; } = true;
    
    public string BackgroundColor { get; set; } = string.Empty;
    
    public string TextColor { get; set; } = string.Empty;
}