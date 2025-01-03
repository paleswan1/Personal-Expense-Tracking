
using PersonalExpenseTracker.Models.Base;
using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.Models;

public class Transaction : BaseEntity<Guid>
{
    public string Title { get; set; }

    public string Note { get; set; }

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public TransactionSource Source { get; set; }
}
