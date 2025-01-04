using PersonalExpenseTracker.DTOs.Transaction;

namespace PersonalExpenseTracker.Components.Pages.Transactions;

public partial class Transactions
{
    protected override void OnInitialized()
    {
        GetTransactionsCount();
    }

    #region Tab Counts
    private int ActivePanelIndex { get; set; }
    private GetTransactionsCountDto TransactionsCount { get; set; } = new();

    private void GetTransactionsCount()
    {
        TransactionsCount = TransactionService.GetTransactionsCount();
    }
    #endregion
    
    #region Component Available Trainings Update on Count 
    private void HandleTransactionCounts()
    {
        GetTransactionsCount();
        
        StateHasChanged();
    }
    #endregion
}