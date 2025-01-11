using Color = MudBlazor.Color;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Layout;

public partial class ModalLayout<T> : ComponentBase where T : class
{
    /// <summary>
    /// Parameters for the modal dialog
    /// </summary>
    [Parameter] public string Module { get; set; } = "";

    [Parameter] public string Title { get; set; } = "";

    [Parameter] public string Description { get; set; } = "";

    [Parameter] public Color SubmitColor { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public string CancelLabel { get; set; } = "";

    [Parameter] public string? SubmitLabel { get; set; }

    [Parameter] public string Size { get; set; } = "";

    [Parameter] public string Alignment { get; set; } = "End";

    [Parameter] public EventCallback OnCancel { get; set; }
    
    private T ModelInstance { get; set; } = Activator.CreateInstance<T>();
}