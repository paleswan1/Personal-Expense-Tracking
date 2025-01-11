using Cashify.Application.Interfaces.Dependency;
using MudBlazor;

namespace Cashify.Application.Interfaces.Services;

public interface ISnackbarService : IScopedService
{
    void ShowSnackbar(string message, Severity severity, Variant variant);
}