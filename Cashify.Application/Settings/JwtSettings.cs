using Cashify.Application.Interfaces.Dependency;

namespace Cashify.Application.Settings;

public class JwtSettings : IScopedService
{
    public string Key { get; set; } = string.Empty;
    
    public string Issuer { get; set; } = string.Empty;
        
    public string Audience { get; set; } = string.Empty;
    
    public double AccessTokenExpirationInMinutes { get; set; }

    public double RefreshTokenExpirationInDays { get; set; }
}