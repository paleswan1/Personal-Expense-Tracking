using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Seed;

namespace PersonalExpenseTracker.Services;

public class SeedService(IGenericRepository genericRepository) : ISeedService
{
    public void SeedDefaultTags()
    {
        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        if (!tags.Any(x => x.IsDefault))
        {
            tags.Add(new Tag()
            {
                Name = "Yearly",
                IsDefault = true,
                BackgroundColor = "#007bff",
                TextColor = "#fff"
            });
            
            tags.Add(new Tag()
            {
                Name = "Monthly",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Food",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Drinks",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Clothes",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Gadgets",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Miscellaneous",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Fuel",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Rent",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "EMI",
                IsDefault = true
            });
            
            tags.Add(new Tag()
            {
                Name = "Party",
                IsDefault = true
            });
        }
    }
}