using System.Text;
using System.Security.Claims;
using ExpenseTracker.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using ExpenseTracker.Application.Utility;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Application.Settings;
using ExpenseTracker.Domain.Common.Constants;
using ExpenseTracker.Application.DTOs.Authentication;
using ExpenseTracker.Application.Interfaces.Managers;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Application.Interfaces.Repository;

namespace ExpenseTracker.Infrastructure.Implementations.Services;

public class AuthenticationService(IGenericRepository genericRepository, ILocalStorageManager localStorageManager, IConfiguration configuration) : IAuthenticationService
{
    public int GetUsersCount()
    {
        return genericRepository.GetCount<User>();
    }

    public async Task Register(RegistrationRequestDto registrationRequest)
    {
        var user = genericRepository.GetFirstOrDefault<User>(x => x.Username == registrationRequest.Username);
        
        if (user != null) throw new Exception("A user with the respective username already exists, please try again :)");

        var userModel = new User()
        {
            Username = registrationRequest.Username,
            Password = registrationRequest.Password.Hash(),
            Currency = registrationRequest.Currency
        };

        await genericRepository.Insert(userModel);
    }

    public async Task Login(LoginRequestDto loginRequest)
    {
        var user = genericRepository.GetFirstOrDefault<User>(x => x.Username == loginRequest.Username);

        if (user == null) throw new Exception("A user with the following username does not exist, please try again :)");
        
        var isPasswordValid = loginRequest.Password.Verify(user.Password);
        
        if (!isPasswordValid) throw new Exception("The provided password is incorrect, please try again :)");
        
        var jsonTokenSettings = configuration.GetRequiredSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (jsonTokenSettings == null) throw new Exception("JWT Settings are missing in the configuration.");
        
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = Encoding.ASCII.GetBytes(jsonTokenSettings.Key);

        var symmetricSigningKey = new SymmetricSecurityKey(key);

        var signingCredentials = new SigningCredentials(symmetricSigningKey, SecurityAlgorithms.HmacSha256);

        var expirationTime = DateTime.Now.AddMinutes(jsonTokenSettings.AccessTokenExpirationInMinutes);

        var accessToken = new JwtSecurityToken(
            jsonTokenSettings.Issuer,
            jsonTokenSettings.Audience,
            claims: authClaims,
            signingCredentials: signingCredentials,
            expires: expirationTime
        );

        var token = new JwtSecurityTokenHandler().WriteToken(accessToken);

        await localStorageManager.SetItemAsync(Constants.Authentication.Token, token);
    }

    public async Task Logout()
    {
        await localStorageManager.ClearItemAsync(Constants.Authentication.Token);
    }
}