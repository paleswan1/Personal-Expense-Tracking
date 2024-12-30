namespace PersonalExpenseTracker.DTOs.Transaction;

public class UpdateTransactionDto: InsertTransactionDto
{
    public Guid Id { get; set; }
}
