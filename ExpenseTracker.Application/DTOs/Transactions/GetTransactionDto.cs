using ExpenseTracker.Domain.Common.Enum;
using ExpenseTracker.Application.DTOs.Tags;

namespace ExpenseTracker.Application.DTOs.Transactions;

public class GetTransactionDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Note { get; set; }

    public TransactionType Type { get; set; }

    public TransactionSource Source { get; set; }

    public decimal Amount { get; set; }
    
    public string Date { get; set; } = string.Empty;

    public List<GetTagDto> Tags { get; set; } = [];
}