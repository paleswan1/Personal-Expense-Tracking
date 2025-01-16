namespace Cashify.Application.DTOs.Filters.Dashboard;

public class GetTransactionFilterRequestDto
{
    public bool IsAscending { get; set; } = true;

    public int Count { get; set; } = 5;

    public bool IsDisplayedAsBarChart { get; set; } = true;
    
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}