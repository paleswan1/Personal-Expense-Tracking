namespace PersonalExpenseTracker.DTOs.Debts;

public class InsertDebtDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public int Amount { get; set; }

    public string Source { get; set; }

    public DateTime? DueDate { get; set; }
}