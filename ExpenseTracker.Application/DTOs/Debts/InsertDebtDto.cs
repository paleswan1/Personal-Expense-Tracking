namespace ExpenseTracker.Application.DTOs.Debts;

public class InsertDebtDto
{
    public string Title { get; set; } = string.Empty;

    public int Amount { get; set; }

    public Guid SourceId { get; set; }

    public DateTime? DueDate { get; set; }
}