using Color = MudBlazor.Color;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Layout;

public partial class ModalLayout : ComponentBase
{
    /// <summary>
    /// Parameters for the modal dialog
    /// </summary>
    [Parameter] public string Module { get; set; } = "";

    [Parameter] public string Title { get; set; } = "";

    [Parameter] public string Description { get; set; } = "";

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string Size { get; set; } = "";

    [Parameter] public EventCallback OnCancel { get; set; }
}