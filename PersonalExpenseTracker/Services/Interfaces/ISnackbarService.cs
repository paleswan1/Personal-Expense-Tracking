using MudBlazor;

namespace PersonalExpenseTracker.Services.Interfaces;

public interface ISnackbarService
{
    void ShowSnackbar(string message, Severity severity, Variant variant);
}