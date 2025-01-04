using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.Components.Layout;
using PersonalExpenseTracker.DTOs.Debts;

namespace PersonalExpenseTracker.Components.Pages.Debts;

public partial class Debts
{
    protected override async Task OnInitializedAsync()
    {
        SetPageTitle();
        
        await GetDebtsCount();
    }

    #region Page Title
    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Debts";
    }
    #endregion
    
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