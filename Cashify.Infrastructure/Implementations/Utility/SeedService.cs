using Cashify.Application.Utility;
using Cashify.Application.Interfaces.Utility;
using Cashify.Application.Interfaces.Repository;
using Cashify.Domain.Models;

namespace Cashify.Infrastructure.Implementations.Utility;

public class SeedService(IGenericRepository genericRepository) : ISeedService
{
    public void InitializeDefaultDatasets()
    {
        UtilityMethod.InitializeDataDirectory();

        var tagCount = genericRepository.GetCount<Tag>();
        
        if (tagCount != 0) return;
        
        var tags = new List<Tag>
        {
            new() { Title = "Food", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Transport", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Shopping", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Entertainment", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Health", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Bills", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" },
            new() { Title = "Others", IsDefault = true, BackgroundColor = "#007bff", TextColor = "#fff" }
        };

        foreach (var tag in tags)
        {
            genericRepository.Insert(tag, false);
        }
    }
}