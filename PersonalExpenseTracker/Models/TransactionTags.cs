using PersonalExpenseTracker.Models.Base;

namespace PersonalExpenseTracker.Models;

public class TransactionTags: BaseEntity<Guid>    
{
    public Guid TransactionId { get; set; }

    public Guid TagId { get; set; }
}
