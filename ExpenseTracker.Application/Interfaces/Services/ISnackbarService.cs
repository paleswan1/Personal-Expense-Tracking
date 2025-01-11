using ExpenseTracker.Application.Interfaces.Dependency;
using MudBlazor;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface ISnackbarService : IScopedService
{
    void ShowSnackbar(string message, Severity severity, Variant variant);
}