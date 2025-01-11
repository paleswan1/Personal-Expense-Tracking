namespace Cashify.Application.DTOs.Dashboard;

public class GetDashboardCount
{
    public int TotalInflowsCount { get; set; }
    
    public decimal TotalInflowsAmount { get; set; }
    
    public int TotalOutflowsCount { get; set; }
    
    public decimal TotalOutflowsAmount { get; set; }
    
    public int TotalDebtsCount { get; set; }
    
    public decimal TotalDebtsAmount { get; set; }
    
    public int TotalPendingDebtsCount { get; set; }
    
    public decimal TotalPendingDebtsAmount { get; set; }

    public int TotalClearedDebtsCount { get; set; }

    public decimal TotalClearedDebtsAmount { get; set; }
}