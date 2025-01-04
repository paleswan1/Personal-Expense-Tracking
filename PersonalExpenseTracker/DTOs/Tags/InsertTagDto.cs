using MudBlazor.Utilities;
using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.DTOs.Tags;

public class InsertTagDto
{
    public string Name { get; set; }

    public MudColor BackgroundColor { get; set; } = new();

    public MudColor TextColor { get; set; } = new();
}
