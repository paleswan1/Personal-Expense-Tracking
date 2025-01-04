using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.Components.Layout;
using PersonalExpenseTracker.DTOs.Transaction;

namespace PersonalExpenseTracker.Components.Pages.Transactions;

public partial class Transactions
{
    protected override async Task OnInitializedAsync()
    {
        SetPageTitle();
        
        await GetTransactionsCount();
    }

    #region Page Title
    [CascadingParameter] public MainLayout Layout { get; set; } = new();

    private void SetPageTitle()
    {
        Layout.PageTitle = "Transactions";
    }
    #endregion
    
    #region Tab Counts
    private int ActivePanelIndex { get; set; }
    private GetTransactionsCountDto TransactionsCount { get; set; } = new();

    private async Task GetTransactionsCount()
    {
        TransactionsCount = await TransactionService.GetTransactionsCount();
    }
    #endregion
    
    #region Component Available Trainings Update on Count 
    private async Task HandleTransactionCounts()
    {
        await GetTransactionsCount();
        
        StateHasChanged();
    }
    #endregion
}