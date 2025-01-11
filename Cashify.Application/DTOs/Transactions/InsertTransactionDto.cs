using Cashify.Domain.Common.Enum;

namespace Cashify.Application.DTOs.Transactions;

public class InsertTransactionDto
{
    public string Title { get; set; } = string.Empty;

    public string? Note { get; set; }

    public TransactionType Type { get; set; } = TransactionType.None;

    public TransactionSource Source { get; set; } = TransactionSource.None;

    public decimal Amount { get; set; }

    public List<Guid> TagIds { get; set; } = [];
}