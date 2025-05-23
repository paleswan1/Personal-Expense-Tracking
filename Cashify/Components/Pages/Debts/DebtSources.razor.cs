﻿using MudBlazor;
using Cashify.Application.DTOs.Sources;
using Cashify.Application.DTOs.Filters.Sources;

namespace Cashify.Components.Pages.Debts;

public partial class DebtSources
{
    protected override async Task OnInitializedAsync()
    {
        await GetAllDebtSources();
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

        await GetAllDebtSources();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetSourceDto.Title);
    
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

        await GetAllDebtSources();
    }

    private string GetSortIcon(string column)
    {
        if (CurrentSortColumn != column)
        {
            return Icons.Material.Filled.UnfoldMore;
        }

        return IsSortDescending ? Icons.Material.Filled.ArrowDownward : Icons.Material.Filled.ArrowUpward;
    }

    #endregion

    #region DebtSource Details

    private List<GetSourceDto> DebtSourceModels { get; set; } = new();

    private async Task GetAllDebtSources()
    {
        var filterRequest = new GetSourceFilterRequestDto
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending,
        };

        DebtSourceModels = await SourceService.GetAllSources(filterRequest);

        StateHasChanged();
    }

    #endregion

    #region Debt Source Creation
    private bool IsInsertDebtSourceModalOpen { get; set; }
    private InsertSourceDto DebtSourceModel { get; set; } = new();

    private void OpenCloseInsertDebtSourceModal()
    {
        IsInsertDebtSourceModalOpen = !IsInsertDebtSourceModalOpen;

        DebtSourceModel = new InsertSourceDto();

        StateHasChanged();
    }

    private async Task InsertDebtSource()
    {
        try
        {
            await SourceService.InsertSource(DebtSourceModel);

            OpenCloseInsertDebtSourceModal();

            await GetAllDebtSources();

            SnackbarService.ShowSnackbar("Source successfully created.", Severity.Success, Variant.Outlined);
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}