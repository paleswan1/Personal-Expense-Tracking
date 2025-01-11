using MudBlazor.Utilities;

namespace Cashify.Application.DTOs.Tags;

public class InsertTagDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public MudColor BackgroundColor { get; set; } = new();

    public MudColor TextColor { get; set; } = new();
}