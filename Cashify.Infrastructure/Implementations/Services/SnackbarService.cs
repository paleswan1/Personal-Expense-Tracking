using MudBlazor;
using Cashify.Application.Interfaces.Services;

namespace Cashify.Infrastructure.Implementations.Services;

public class SnackbarService(ISnackbar snackbar) : ISnackbarService
{
    public void ShowSnackbar(string message, Severity severity, Variant variant)
    {
        snackbar.Add(message, severity, c => c.SnackbarVariant = variant);
    }
}