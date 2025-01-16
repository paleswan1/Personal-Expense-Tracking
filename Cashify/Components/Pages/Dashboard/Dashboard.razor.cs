using Cashify.Application.DTOs.Debts;

namespace Cashify.Components.Pages.Dashboard;

public partial class Dashboard
{
    protected override async Task OnInitializedAsync()
    {
        await GetDebtsCount();
    }
    
    #region Debt Tab Counts
    private int ActivePanelIndex { get; set; }

    private GetDebtsCountDto DebtsCount { get; set; } = new();

    private async Task GetDebtsCount()
    {
        DebtsCount = await DebtService.GetDebtsCount();
    }
    #endregion
}