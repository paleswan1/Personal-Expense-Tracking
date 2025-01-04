using MudBlazor;
using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Filters.Tags;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.DTOs.Transaction;
using PersonalExpenseTracker.Filters.Transactions;

namespace PersonalExpenseTracker.Components.Pages.Transactions;

public partial class TransactionDetails
{
    [Parameter] 
    public TransactionType? TransactionType { get; set; }

    [Parameter] public EventCallback OnTransactionsCountUpdate { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await GetAllTags();

        await GetBalanceDetails();
        
        await GetAllTransactions();
    }

    #region Search with Filter and Order
    private string _search = string.Empty;

    private string Search
    {
        get => _search;
        set
        {
            if (_search == value) return;
            _search = value;
            _ = OnSearchInputAsync(_search);
        }
    }

    private async Task OnSearchInputAsync(string search)
    {
        Search = search;

        await GetAllTransactions();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetTransactionDto.Title);

    private bool IsSortDescending { get; set; }

    private async Task ChangeSorting(string column)
    {
        if (CurrentSortColumn == column)
        {
            IsSortDescending = !IsSortDescending;
        }
        else
        {
            CurrentSortColumn = column;
            IsSortDescending = false;
        }

        await GetAllTransactions();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }

    private IEnumerable<Guid> FilterTagIdentifiers { get; set; } = [];
    
    private DateTime? StartDate { get; set; }

    private DateTime? EndDate { get; set; }

    private async Task OnTransactionFilterHandler()
    {
        await GetAllTransactions();
    }
    #endregion

    #region Balance
    private decimal Balance { get; set; }

    private async Task GetBalanceDetails()
    {
        Balance = await TransactionService.GetRemainingBalance();
    }
    #endregion

    #region Transaction Details
    private List<GetTransactionDto> TransactionModels { get; set; } = new();

    private async Task GetAllTransactions()
    {
        var filterRequest = new GetTransactionFilterRequestDto
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
            TagIds = FilterTagIdentifiers.ToList(),
            StartDate = StartDate,
            EndDate = EndDate,
            TransactionType = TransactionType
        };

        TransactionModels = await TransactionService.GetAllTransactions(filterRequest);

        StateHasChanged();
    }
    #endregion

    #region Transaction Creation
    private bool IsInsertTransactionModalOpen { get; set; }
    
    private List<GetTagDto> Tags { get; set; } = [];

    private IEnumerable<Guid> TagIdentifiers { get; set; } = [];
    
    private InsertTransactionDto InsertTransactionModel { get; set; } = new();

    private async Task GetAllTags()
    {
        Tags = await TagService.GetAllTags(new GetTagFilterRequestDto());
    }
    
    private void OpenCloseInsertTransactionModal()
    {
        IsInsertTransactionModalOpen = !IsInsertTransactionModalOpen;

        InsertTransactionModel = new InsertTransactionDto();

        TagIdentifiers = [];
        
        StateHasChanged();
    }

    private async Task InsertTransaction()
    {
        try
        {
            InsertTransactionModel.TagIds = TagIdentifiers.ToList();
            
            await TransactionService.InsertTransaction(InsertTransactionModel);

            await OnTransactionsCountUpdate.InvokeAsync();

            OpenCloseInsertTransactionModal();

            await GetAllTransactions();

            await GetBalanceDetails();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion

    #region Transaction Note
    private UpdateTransactionDto UpdateTransactionModel { get; set; } = new();

    private bool IsUpdateTransactionNoteModalOpen { get; set; }

    private void OpenCloseUpdateTransactionNoteModal(Guid transactionId)
    {
        IsUpdateTransactionNoteModalOpen = !IsUpdateTransactionNoteModalOpen;

        var transactionDetails = TransactionService.GetTransactionById(transactionId);

        UpdateTransactionModel = new UpdateTransactionDto()
        {
            Id = transactionDetails.Id,
            Amount = transactionDetails.Amount,
            Note = transactionDetails.Note,
            Title = transactionDetails.Title,
            Source = transactionDetails.Source,
            Type = transactionDetails.Type,
            TagIds = transactionDetails.Tags.Select(x => x.Id).ToList()
        };
        
        StateHasChanged();
    }

    private async Task UpdateTransactionNote()
    {
        try
        {
            await TransactionService.UpdateTransaction(UpdateTransactionModel);

            OpenCloseUpdateTransactionNoteModal(UpdateTransactionModel.Id);
            
            await GetAllTransactions();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}