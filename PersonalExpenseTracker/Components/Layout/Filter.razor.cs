using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace PersonalExpenseTracker.Components.Layout;

public partial class Filter
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
   
    [Parameter] public EventCallback OnFilterApplication { get; set; }

    private MudMenu FilterMenu { get; set; } = new();
    
    private async Task OnApplyFilter(bool isFilterApplied)
    {
        if (isFilterApplied) await OnFilterApplication.InvokeAsync();
        
        await FilterMenu.OpenChanged.InvokeAsync(false);
        
        StateHasChanged();
    }
}