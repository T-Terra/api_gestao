namespace Expenses.Config;

public class Configuration
{
    private readonly IConfiguration _config;
    
    public Configuration(IConfiguration config)
    {
        _config = config;
    }
    public string GetPrivateKey()
    {
        var settings = _config.GetSection("JwtSettings");
        var privateKey = settings["SecretKey"] ?? string.Empty;
        
        return privateKey;
    }
    public int GetExpires()
    {
        var settings = _config.GetSection("JwtSettings");
        var expires = settings.GetValue<int>("ExpirationTimeInMinutes");
        
        return expires;
    }

    public int GetExpiresRefresh()
    {
        var settings = _config.GetSection("JwtSettings");
        var expires = settings.GetValue<int>("RefreshExpirationTimeInMinutes");
        
        return expires;
    }

    public string GetIssuer()
    {
        var settings = _config.GetSection("JwtSettings");
        var issuer = settings.GetValue<string>("Issuer") ?? string.Empty;
        
        return issuer;
    }

    public string GetAudience()
    {
        var settings = _config.GetSection("JwtSettings");
        var audience = settings.GetValue<string>("Audience") ?? string.Empty;
        return audience;
    }
}