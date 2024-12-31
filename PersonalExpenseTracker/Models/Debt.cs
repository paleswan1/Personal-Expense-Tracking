using PersonalExpenseTracker.Models.Base;

namespace PersonalExpenseTracker.Models;

public class Debt: BaseEntity<Guid>
{
    public string Title { get; set; }

    public string Source { get; set; }

    public int Amount { get; set; }

    public DateTime DueDate { get; set; }

    public bool Status { get; set; }

}
