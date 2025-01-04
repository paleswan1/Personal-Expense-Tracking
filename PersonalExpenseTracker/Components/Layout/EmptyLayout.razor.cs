using MudBlazor;

namespace PersonalExpenseTracker.Components.Layout;

public partial class EmptyLayout
{
    private MudTheme Theme { get; } = new ()
    {
        ZIndex = new ZIndex
        {
            Drawer = 1300
        }
    };
}