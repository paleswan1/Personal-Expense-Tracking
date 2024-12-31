using PersonalExpenseTracker.Models.Base;

namespace PersonalExpenseTracker.DTOs.Debts;

public class GetDebtDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public int Amount   { get; set; }   

    public string Source { get; set; }

    public bool Status { get; set; }

    public DateTime DueDate { get; set; }

}
