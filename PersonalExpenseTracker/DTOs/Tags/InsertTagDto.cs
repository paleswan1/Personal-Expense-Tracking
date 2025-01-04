using MudBlazor.Utilities;

namespace PersonalExpenseTracker.DTOs.Tags;

public class InsertTagDto
{
    public string Name { get; set; }

    public MudColor BackgroundColor { get; set; } = new();

    public MudColor TextColor { get; set; } = new();
}
