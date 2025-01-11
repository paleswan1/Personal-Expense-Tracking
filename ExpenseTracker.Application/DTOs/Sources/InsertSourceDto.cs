using MudBlazor.Utilities;

namespace ExpenseTracker.Application.DTOs.Sources;

public class InsertSourceDto
{
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public MudColor BackgroundColor { get; set; } = new();

    public MudColor TextColor { get; set; } = new();
}
