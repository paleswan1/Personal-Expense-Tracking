namespace ExpenseTracker.Application.DTOs.Sources;

public class GetSourceDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string BackgroundColor { get; set; } = string.Empty;

    public string TextColor { get; set; } = string.Empty;
}
