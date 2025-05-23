﻿using MudBlazor;
using Cashify.Domain.Common.Enum;
using Cashify.Application.DTOs.Debts;
using Microsoft.AspNetCore.Components;
using Cashify.Application.DTOs.Transactions;
using Cashify.Application.DTOs.Filters.Debts;
using Cashify.Application.DTOs.Filters.Sources;
using Cashify.Application.DTOs.Sources;

namespace Cashify.Components.Pages.Debts;

public partial class DebtDetails
{
    [Parameter] 
    public DebtStatus? DebtStatus { get; set; }
    
    [Parameter] 
    public bool IsEditable { get; set; }

    [Parameter] 
    public EventCallback OnDebtsCountUpdate { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await GetAllDebts();

        await GetAllDebtSources();
        
        await GetBalanceAndDebtDetails();
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

        await GetAllDebts();
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

        await GetAllDebts();
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
        
        await GetAllDebts();
    }
    #endregion

    #region Balance
    private decimal Balance { get; set; }

    private decimal PendingDebtAmount { get; set; }

    private async Task GetBalanceAndDebtDetails()
    {
        Balance = await TransactionService.GetRemainingBalance();

        PendingDebtAmount = await DebtService.GetPendingDebtAmounts();
    }
    #endregion

    #region Debt Sources
    private List<GetSourceDto> DebtSourceModels { get; set; } = [];

    private async Task GetAllDebtSources()
    {
        try
        {
            DebtSourceModels = await SourceService.GetAllSources(new GetSourceFilterRequestDto());
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
    
    #region Debts Details
    private List<GetDebtDto> DebtModels { get; set; } = new();

    private async Task GetAllDebts()
    {
        var filterRequest = new GetDebtFilterRequestDto()
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
            StartDate = StartDate,
            EndDate = EndDate,
            Status = DebtStatus
        };

        DebtModels = await DebtService.GetAllDebts(filterRequest);

        StateHasChanged();
    }
    #endregion

    #region Debts Creation
    private bool IsInsertDebtModalOpen { get; set; }
    
    private InsertDebtDto InsertDebtModel { get; set; } = new();
    
    private void OpenCloseInsertDebtModal()
    {
        IsInsertDebtModalOpen = !IsInsertDebtModalOpen;

        InsertDebtModel = new InsertDebtDto();
        
        StateHasChanged();
    }

    private async Task InsertDebt()
    {
        try
        {
            await DebtService.InsertDebt(InsertDebtModel);

            await OnDebtsCountUpdate.InvokeAsync();
            
            await GetBalanceAndDebtDetails();
            
            OpenCloseInsertDebtModal();
            
            await GetAllDebts();
            
            SnackbarService.ShowSnackbar("Debt successfully created.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion

    #region Clear Debt
    private GetDebtDto GetDebtModel { get; set; } = new();

    private bool IsClearDebtModalOpen { get; set; }

    private void OpenCloseClearDebtModal(Guid debtId)
    {
        IsClearDebtModalOpen = !IsClearDebtModalOpen;

        GetDebtModel = IsClearDebtModalOpen ? DebtService.GetDebtById(debtId) : new();

        StateHasChanged();
    }

    private async Task ClearDebt()
    {
        try
        {
            await DebtService.ClearDebt(GetDebtModel.Id);

            OpenCloseClearDebtModal(GetDebtModel.Id);

            await OnDebtsCountUpdate.InvokeAsync();
            
            await GetBalanceAndDebtDetails();

            await GetAllDebts();
            
            SnackbarService.ShowSnackbar("Debt successfully cleared.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}