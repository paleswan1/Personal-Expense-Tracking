using Cashify.Application.DTOs.Dashboard;
using Cashify.Application.DTOs.Filters.Dashboard;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Cashify.Components.Pages.Dashboard;

public partial class TransactionCharts : ComponentBase
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await GetInflowsTransactionDetails(true);
            await GetOutflowsTransactionDetails(true);
            await GetDebtsTransactionDetails(true);
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    #region Transaction Chart Details
    private bool IsInflowsDisplayedAsBarChart { get; set; }

    private bool IsOutflowsDisplayedAsBarChart { get; set; }
    
    private bool IsDebtsDisplayedAsBarChart { get; set; }
    
    private GetTransactionFilterRequestDto InflowsFilter { get; set; } = new();
    
    private List<GetTransactionDetails> InflowsData { get; set; } = [];
    
    private GetTransactionFilterRequestDto OutflowsFilter { get; set; } = new();

    private List<GetTransactionDetails> OutflowsData { get; set; } = [];
    
    private GetTransactionFilterRequestDto DebtsFilter { get; set; } = new();

    private List<GetTransactionDetails> DebtsData { get; set; } = [];

    private async Task GetInflowsTransactionDetails(bool isFiltered)
    {
        try
        {
            InflowsFilter = isFiltered ? InflowsFilter : new GetTransactionFilterRequestDto();

            InflowsData = await DashboardService.GetInflowsTransactions(InflowsFilter);

            IsInflowsDisplayedAsBarChart = InflowsFilter.IsDisplayedAsBarChart;
            
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    
    private async Task GetOutflowsTransactionDetails(bool isFiltered)
    {
        try
        {
            OutflowsFilter = isFiltered ? OutflowsFilter : new GetTransactionFilterRequestDto();
            
            OutflowsData = await DashboardService.GetOutflowsTransactions(OutflowsFilter);
            
            IsOutflowsDisplayedAsBarChart = OutflowsFilter.IsDisplayedAsBarChart;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    
    private async Task ClearDebtsFilter()
    {
        InflowsFilter = new GetTransactionFilterRequestDto();
        
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task GetDebtsTransactionDetails(bool isFiltered)
    {
        try
        {
            DebtsFilter = isFiltered ? DebtsFilter : new GetTransactionFilterRequestDto();

            DebtsData = await DashboardService.GetDebtsTransactions(DebtsFilter);

            IsDebtsDisplayedAsBarChart = DebtsFilter.IsDisplayedAsBarChart;

            StateHasChanged();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}