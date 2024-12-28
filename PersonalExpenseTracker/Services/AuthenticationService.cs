using PersonalExpenseTracker.DTOs.Authentication;
using PersonalExpenseTracker.Managers;
using PersonalExpenseTracker.Models;
using PersonalExpenseTracker.Repositories;
using PersonalExpenseTracker.Services.Interfaces;

namespace PersonalExpenseTracker.Services;

public class AuthenticationService(IGenericRepository genericRepository, ISerializeDeserializeManager serializeDeserializeManager, ILocalStorageManager localStorageManager) : IAuthenticationService 
{
    private static string appDataDirectoryPath = ExtensionMethods.GetAppDirectoryPath();
    private static string appUsersFilePath = ExtensionMethods.GetAppUsersFilePath();

    /// <summary>
    /// Retrieves users list from the respective user load file path
    /// </summary>
    /// <returns>List must have atleast one record. Therefore, it returns whether a user is registered or not.</returns>
    public bool IsUserRegistered()
    {
        var users = genericRepository.GetAll<User>(appUsersFilePath);
        return users != null && users.Any();
    }

    public UserDetailsDto? Login(LoginRequestDto login)
    {
        var users = genericRepository.GetAll<User>(appUsersFilePath);

        var user = users.FirstOrDefault(x => x.Username == login.Username.Trim().ToLower());

        if (user == null)
        {
            throw new Exception();
        }

        // var isPasswordValid = ExtensionMethods.VerifyHash(login.Password, user.PasswordHash);

        var isPasswordValid = login.Password.VerifyHash(user.PasswordHash);

        if(!isPasswordValid)
        {
            throw new Exception();
        }

        // This is the object of userDetails that will be converted into json object. 
        var result = new UserDetailsDto
        {
            Id = user.Id,
            Name = user.Name,
            Currency = user.Currency,
            UserName = user.Username,
        };

        var userDetails = new List<UserDetailsDto>()
        {
            result
        };

        var serializedUserDetails = serializeDeserializeManager.Serialize(userDetails);

        localStorageManager.SetItemAsync("user_details", serializedUserDetails);

        return result;
    }

    public void Register(RegisterRequestDto register)
    {
        register.UserName = register.UserName.Trim();

        if(register.UserName == "" || register.Currency == "" || register.Password == "")
        {
            throw new Exception("Please insert correct and valid input for each of the fields.");
        }

        var users = genericRepository.GetAll<User>(appUsersFilePath);

        var usernameExists = users.Any(x => x.Username == register.UserName);

        if (usernameExists)
        {
            throw new Exception("Username already exists. Please choose any other username.");
        }

        var user = new User()
        {
            Username = register.UserName,
            PasswordHash = ExtensionMethods.HashSecret(register.Password),
            Currency = register.Currency,
            CreatedAt = DateTime.Now,
            IsActive = true,
            Name = register.UserName
        };

        users.Add(user);

        genericRepository.SaveAll(users, appDataDirectoryPath, appUsersFilePath);
    }
}
