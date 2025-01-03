namespace PersonalExpenseTracker.Models.Constant;

public enum TransactionType
{
    None = 0,
    Inflows = 1, 
    Outflows = 2
}

public enum TransactionSource
{
    None = 0,
    Credit = 1,
    Gain = 2,
    Budget = 3,
    Debit = 4,
    Spending = 5,
    Expenses = 6
}

public enum Currency
{
    None = 0,
    NRP = 1,
    INR = 2,
    USD = 3,
    AUD = 4,
}

public enum DebtStatus
{
    None = 0,
    Cleared = 1,
    Pending = 2,
    PastDue = 3
}