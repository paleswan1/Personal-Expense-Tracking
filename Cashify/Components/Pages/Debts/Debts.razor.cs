using Cashify.Application.DTOs.Debts;
using Cashify.Components.Layout;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Pages.Debts;

public partial class Debts
{
    protected override async Task OnInitializedAsync()
    {
        await GetDebtsCount();
    }

    #region Tab Counts
    private int ActivePanelIndex { get; set; }

    private GetDebtsCountDto DebtsCount { get; set; } = new();

    private async Task GetDebtsCount()
    {
        DebtsCount = await DebtService.GetDebtsCount();
    }
    #endregion

    #region Component Available Trainings Update on Count
    private async Task HandleDebtCounts()
    {
        await GetDebtsCount();

        StateHasChanged();
    }
    #endregion
}