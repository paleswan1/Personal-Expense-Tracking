using Cashify.Domain.Common.Base;

namespace Cashify.Domain.Models;

public class TransactionTags : BaseEntity
{
    public Guid TransactionId { get; set; }

    public Guid TagId { get; set; }
}