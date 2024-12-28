using PersonalExpenseTracker.DTOs.Authentication;
using PersonalExpenseTracker.Managers;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class UserService(ISerializeDeserializeManager serializeDeserializeManager, ILocalStorageManager localStorageManager) : IUserService
{
    public UserDetailsDto? GetUserDetails()
    {
        var userDetails = localStorageManager.GetItemAsync<string>("user_details");

        if (userDetails == null || userDetails.Result == null)
        {
            return null;
        }

        var deserializedUserDetails = serializeDeserializeManager.Deserialize<UserDetailsDto>(userDetails.Result);

        if (deserializedUserDetails.Count == 0)
        {
            return null;
        }

        var result = deserializedUserDetails.FirstOrDefault();

        return result;
        
    }

}
