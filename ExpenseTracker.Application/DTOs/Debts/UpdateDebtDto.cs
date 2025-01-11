namespace ExpenseTracker.Application.DTOs.Debts;

public class UpdateDebtDto : InsertDebtDto
{
    public Guid Id { get; set; }
}