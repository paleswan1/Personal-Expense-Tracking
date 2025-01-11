using Cashify.Application.DTOs.Transactions;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Pages.Transactions;

public partial class Transactions
{
    protected override async Task OnInitializedAsync()
    {
        await GetTransactionsCount();
    }

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