using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Interfaces.Managers;

public interface ICsvManager : ITransientService
{
    string GenerateCsv<T>(List<T> data);

    Task<string> SaveCsvFileAsync(string fileName, string csvContent);
}