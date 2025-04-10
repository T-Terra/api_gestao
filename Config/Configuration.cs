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
}