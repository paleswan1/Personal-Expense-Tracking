using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Application.Settings;
using ExpenseTracker.Domain.Common.Constants;
using ExpenseTracker.Application.Interfaces.Utility;
using ExpenseTracker.Application.Interfaces.Managers;

namespace ExpenseTracker.Infrastructure.Implementations.Utility;

public class UserService(ILocalStorageManager localStorageManager, IConfiguration configuration) : IUserService
{
    public async Task<Guid> GetUserId()
    {
        var settings = configuration.GetRequiredSection(nameof(JwtSettings)).Get<JwtSettings>();

        if (settings == null) throw new Exception("JWT Settings are missing in the configuration.");

        var encryptedToken = await localStorageManager.GetItemAsync<string>(Constants.Authentication.Token);

        if (string.IsNullOrWhiteSpace(encryptedToken)) return Guid.Empty;

        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(encryptedToken)) return Guid.Empty;

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = settings.Issuer,
                ValidateAudience = true,
                ValidAudience = settings.Audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                ValidateIssuerSigningKey = true
            };

            handler.ValidateToken(encryptedToken, validationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtToken)
            {
                var userIdentifier = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdentifier)) return Guid.Empty;

                Console.WriteLine($"User Identifier: {userIdentifier}.");

                return Guid.Parse(userIdentifier);
            }

            Console.WriteLine("Token could not be casted to a valid JWT Security Token.");
        }
        catch (SecurityTokenValidationException ex)
        {
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while validating the token: {ex.Message}");
        }

        return Guid.Empty;
    }
}