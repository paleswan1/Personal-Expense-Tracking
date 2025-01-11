namespace Cashify.Application.DTOs.Tags;

public class GetTagDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string BackgroundColor { get; set; } = string.Empty;

    public string TextColor { get; set; } = string.Empty;

    public bool IsDefault { get; set; }
}