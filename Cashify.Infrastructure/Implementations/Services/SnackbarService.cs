using MudBlazor;
using Cashify.Application.Interfaces.Services;

namespace Cashify.Infrastructure.Implementations.Services;

/// <summary>
/// Provides services for displaying notifications using the MudBlazor Snackbar component.
/// </summary>
/// <param name="snackbar">MudBlazor snackbar instance for displaying messages.</param>
public class SnackbarService(ISnackbar snackbar) : ISnackbarService
{
    /// <summary>
    /// Displays a snackbar notification with the specified message, severity, and visual variant.
    /// </summary>
    /// <param name="message">The message to be displayed in the snackbar.</param>
    /// <param name="severity">The severity level of the snackbar (e.g., Info, Warning, Error).</param>
    /// <param name="variant">The visual variant of the snackbar (e.g., Text, Filled, Outlined).</param>
    public void ShowSnackbar(string message, Severity severity, Variant variant)
    {
        snackbar.Add(message, severity, c => c.SnackbarVariant = variant);
    }
}