using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.DTOs.Transaction;

public class GetTransactionDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Note { get; set; }

    public TransactionType Type { get; set; }

    public TransactionSource Source { get; set; }

    public decimal Amount { get; set; }

    public List<GetTagDto> Tags { get; set; }  
}
