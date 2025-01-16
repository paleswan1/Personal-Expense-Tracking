using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace Cashify.Components.Layout;

public partial class DonutChart<T> : ComponentBase where T : class
{
    [Parameter]
    public List<T> Items { get; set; } = new();

    [Parameter] 
    public Func<T, string> XValue { get; set; }

    [Parameter]
    public Func<T, decimal?> YValue { get; set; }

    private ApexChart<T>? _chart;

    private ApexChartOptions<T> ChartOptions { get; set; } = new();

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
        ChartOptions = new ApexChartOptions<T>
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
                Pie = new PlotOptionsPie
                {
                    Donut = new PlotOptionsDonut
                    {
                        Labels = new DonutLabels
                        {
                            Total = new DonutLabelTotal 
                            { 
                                Show = true,
                                FontSize = "24px",
                                Color = "#D807B8",
                                Formatter = @"function (w) {
                                    return w.globals.seriesTotals.reduce((a, b) => a + b, 0);
                                }"
                            }
                        }
                    }
                }
            }
        };
    }
}