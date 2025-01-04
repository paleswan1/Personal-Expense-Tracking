namespace PersonalExpenseTracker.DTOs.Transaction;

public class GetTransactionsCountDto
{
    public int AllCount { get; set; }
    
    public int InflowsCount { get; set; }

    public int OutflowsCount { get; set; }
}
