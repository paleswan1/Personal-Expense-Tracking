using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Layout;

public partial class BarChart<T> : ComponentBase where T : class
{
    [Parameter] public List<T> Items { get; set; } = [];
    
    [Parameter] public Func<T, string> XValue { get; set; }
    
    [Parameter] public Func<IEnumerable<T>, Decimal?> YValue { get; set; }

    [Parameter] public bool IsHorizontallyAligned { get; set; } = true;

    [Parameter] public string Height { get; set; } = "320";

    [Parameter] public string Title { get; set; } = "Value";

    [Parameter] public bool ShowLegend { get; set; } = true;
    
    private ApexChartOptions<T> Options { get; set; }
    private ApexChart<T>? _chart;

    protected override void OnInitialized()
    {
        InitializeOptions();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_chart != null)
        {
            await _chart.UpdateSeriesAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void InitializeOptions()
    {
        Options = new ApexChartOptions<T>
        {
            Chart = new Chart
            {
                Animations = new Animations
                {
                    Enabled = true,
                    DynamicAnimation = new DynamicAnimation
                    {
                        Enabled = true
                    }
                }
            },
            PlotOptions = new PlotOptions
            {
                Bar = new PlotOptionsBar
                {
                    Horizontal = IsHorizontallyAligned,
                    Distributed = true,
                    ColumnWidth = "10%"
                }
            },
            Legend = new()
            {
                Show = ShowLegend
            },
            Colors = new List<string> { "#005399", "#ff0000", "#00cc29", "#0bc5ea", "#775DD0" },
        };
    }
}