namespace ExpenseTracker.Domain.Common.Base;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool IsActive { get; set; } = true;

    public Guid CreatedBy { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Guid? LastUpdatedBy { get; set; }

    public DateTime? LastUpdatedDate { get; set; }
}