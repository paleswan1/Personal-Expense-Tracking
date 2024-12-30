using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.DTOs.Transaction;

public class InsertTransactionDto
{
    public string Title { get; set; }

    public string Note { get; set; }

    public TransactionType Type { get; set; }

    public TransactionSource Source { get; set; }

    public int Amount { get; set; }

    public List<Guid> TagIds { get; set; }
}

