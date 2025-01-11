using ExpenseTracker.Domain.Common.Base;
using ExpenseTracker.Domain.Common.Enum;

namespace ExpenseTracker.Domain.Models;

public class User : BaseEntity
{
    public string Username { get; set; }

    public string Password { get; set; }

    public Currency Currency { get; set; }
}