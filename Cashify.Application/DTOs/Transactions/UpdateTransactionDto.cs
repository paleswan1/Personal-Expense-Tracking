namespace Cashify.Application.DTOs.Transactions;

public class UpdateTransactionDto: InsertTransactionDto
{
    public Guid Id { get; set; }
}