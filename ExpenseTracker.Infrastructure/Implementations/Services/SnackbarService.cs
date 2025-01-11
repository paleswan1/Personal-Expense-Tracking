using MudBlazor;
using ExpenseTracker.Application.Interfaces.Services;

namespace ExpenseTracker.Infrastructure.Implementations.Services;

public class SnackbarService(ISnackbar snackbar) : ISnackbarService
{
    public void ShowSnackbar(string message, Severity severity, Variant variant)
    {
        snackbar.Add(message, severity, c => c.SnackbarVariant = variant);
    }
}