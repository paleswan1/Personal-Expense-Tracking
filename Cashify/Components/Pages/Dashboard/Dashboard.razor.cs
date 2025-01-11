using MudBlazor;
using Cashify.Application.DTOs.Debts;
using Cashify.Application.DTOs.Dashboard;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.DTOs.Filters.Dashboard;

namespace Cashify.Components.Pages.Dashboard;

public partial class Dashboard
{
    protected override async Task OnInitializedAsync()
    {
        await GetDashboardCount();
        
        await GetAllPendingDebts();
        
        await GetInflowsTransactionDetails();
        
        await GetOutflowsTransactionDetails();
        
        await GetDebtsTransactionDetails();
    }
    
    #region Dashboard Count
    private GetDashboardCount DashboardCount { get; set; } = new GetDashboardCount();

    private async Task GetDashboardCount()
    {
        try
        {
            DashboardCount = await DashboardService.GetDashboardCount();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
    
    #region Pending Debts
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

        await GetAllPendingDebts();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetDebtDto.Title);

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

        await GetAllPendingDebts();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }

    private DateTime? StartDate { get; set; }

    private DateTime? EndDate { get; set; }

    private async Task OnDebtFilterHandler(bool isFilterApplied)
    {
        if (!isFilterApplied)
        {
            StartDate = null;
            EndDate = null;
        }
        
        await GetAllPendingDebts();
    }

    private GetDebtDto DebtModel { get; set; } = new();
    
    private List<GetDebtDto> PendingDebts { get; set; } = [];

    private async Task GetAllPendingDebts()
    {
        try
        {
            var filterRequest = new GetDebtFilterRequestDto()
            {
                Search = Search,
                OrderBy = CurrentSortColumn,
                IsDescending = IsSortDescending,
                StartDate = StartDate,
                EndDate = EndDate
            };
            
            PendingDebts = await DashboardService.GetPendingDebts(filterRequest);
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion

    #region Transaction Chart Details
    private GetTransactionFilterRequestDto InflowsFilter { get; set; } = new();
    
    private List<GetTransactionDetails> InflowsData { get; set; } = [];
    
    private GetTransactionFilterRequestDto OutflowsFilter { get; set; } = new();

    private List<GetTransactionDetails> OutflowsData { get; set; } = [];
    
    private GetTransactionFilterRequestDto DebtsFilter { get; set; } = new();

    private List<GetTransactionDetails> DebtsData { get; set; } = [];

    private async Task GetInflowsTransactionDetails()
    {
        try
        {
            InflowsData = await DashboardService.GetInflowsTransactions(InflowsFilter);
            
            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    
    private async Task GetOutflowsTransactionDetails()
    {
        try
        {
            OutflowsData = await DashboardService.GetOutflowsTransactions(OutflowsFilter);
            
            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    
    private async Task GetDebtsTransactionDetails()
    {
        try
        {
            DebtsData = await DashboardService.GetDebtsTransactions(DebtsFilter);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}