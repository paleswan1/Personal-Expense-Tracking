using System.Text;
using Cashify.Application.Interfaces.Managers;

namespace Cashify.Infrastructure.Implementations.Managers;

public class CsvManager : ICsvManager
{
    public string GenerateCsv<T>(List<T> data)
    {
        var stringBuilder = new StringBuilder();
        
        var properties = typeof(T).GetProperties();

        stringBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

        foreach (var item in data)
        {
            stringBuilder.AppendLine(string.Join(",", properties.Select(p => p.GetValue(item))));
        }

        return stringBuilder.ToString();
    }

    public async Task<string> SaveCsvFileAsync(string filePath, string csvContent)
    {
        await File.WriteAllTextAsync(filePath, csvContent);

        return filePath;
    }
}