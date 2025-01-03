using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Managers;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Models.Constant;
using PersonalExpenseTracker.DTOs.Authentication;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class UserService(ISerializeDeserializeManager serializeDeserializeManager, ILocalStorageManager localStorageManager, IGenericRepository genericRepository) : IUserService
{
    public UserDetailsDto? GetUserDetails()
    {
        var userDetails = localStorageManager.GetItemAsync<string>("user_details");

        if (userDetails.Result == null)
        {
            return null;
        }

        var deserializedUserDetails = serializeDeserializeManager.Deserialize<UserDetailsDto>(userDetails.Result);

        if (deserializedUserDetails.Count == 0)
        {
            return null;
        }

        var result = deserializedUserDetails.FirstOrDefault();

        var user = genericRepository.GetAll<User>(Constants.FilePath.AppUsersDirectoryPath).FirstOrDefault(x => x.Id == result?.Id);

        if (user == null) return null;
        
        return new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Currency = user.Currency,
            UserName = user.Username
        };
    }
}
