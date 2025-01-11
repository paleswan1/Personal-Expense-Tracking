using Cashify.Domain.Common.Base;
using Cashify.Domain.Common.Enum;

namespace Cashify.Domain.Models;

public class Debt : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    
    public Guid SourceId { get; set; }
    
    public DebtStatus Status { get; set; }

    public DateOnly DueDate { get; set; }

    public DateTime? ClearedDate { get; set; }
}