using MudBlazor;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class SnackbarService(ISnackbar snackbar) : ISnackbarService
{
    public void ShowSnackbar(string message, Severity severity, Variant variant)
    {
        snackbar.Add(message, severity, c => c.SnackbarVariant = variant);
    }
}