using MudBlazor;
using PersonalExpenseTracker.DTOs.Tags;
using PersonalExpenseTracker.Filters.Tags;

namespace PersonalExpenseTracker.Components.Pages.Tags;

public partial class Tags
{
    protected override async Task OnInitializedAsync()
    {
        await GetAllTags();
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

        await GetAllTags();
    }

    private string CurrentSortColumn { get; set; } = nameof(GetTagDto.Name);
    
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

        await GetAllTags();
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

    #region Tag Details
    private List<GetTagDto> TagModels { get; set; } = new();

    private async Task GetAllTags()
    {
        var filterRequest = new GetTagFilterRequestDto
        {
            Search = Search,
            OrderBy = CurrentSortColumn,
            IsDescending = IsSortDescending
        };

        TagModels = await TagService.GetAllTags(filterRequest);
    }
    #endregion

    #region Tag Creation
    private bool IsInsertTagModalOpen { get; set; }

    private InsertTagDto TagModel { get; set; } = new();

    private void OpenCloseInsertTagModal()
    {
        IsInsertTagModalOpen = !IsInsertTagModalOpen;

        TagModel = new InsertTagDto();

        StateHasChanged();
    }

    private async Task InsertTag()
    {
        try
        {
            await TagService.InsertTag(TagModel);

            OpenCloseInsertTagModal();

            await GetAllTags();
        }
        catch (Exception ex)
        {
            SnackbarService.ShowSnackbar(ex.Message, Severity.Error, Variant.Outlined);
        }
    }
    #endregion
}