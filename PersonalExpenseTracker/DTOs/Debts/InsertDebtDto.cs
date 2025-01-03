using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.DTOs.Debts
{
    public class InsertDebtDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Amount { get; set; }

        public string Source { get; set; }

        public DebtStatus Status { get; set; }

        public DateOnly DueDate { get; set; }
    }
}
