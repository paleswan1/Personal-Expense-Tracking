using Cashify.Domain.Common.Enum;

namespace Cashify.Application.DTOs.Filters.Transactions;

public class GetTransactionFilterRequestDto : GetFilterRequestDto
{
    public List<Guid>? TagIds { get; set; }
    
    public TransactionType? TransactionType { get; set; }
    
    public DateTime? StartDate { get; set; } 

    public DateTime? EndDate { get; set; } 
}