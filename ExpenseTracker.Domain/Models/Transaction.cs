using ExpenseTracker.Domain.Common.Base;
using ExpenseTracker.Domain.Common.Enum;

namespace ExpenseTracker.Domain.Models;

public class Transaction : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public TransactionType Type { get; set; }

    public string? Note { get; set; }

    public decimal Amount { get; set; }

    public TransactionSource Source { get; set; }
}