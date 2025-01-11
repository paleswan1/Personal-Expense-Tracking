using ExpenseTracker.Domain.Common.Enum;
using ExpenseTracker.Application.DTOs.Sources;

namespace ExpenseTracker.Application.DTOs.Debts;

public class GetDebtDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Amount   { get; set; }

    public GetSourceDto Source { get; set; } = new();

    public DebtStatus Status { get; set; }

    public string DueDate { get; set; } = string.Empty;

    public string? ClearedDate { get; set; }
}
