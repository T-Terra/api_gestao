using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses.Config;
using Expenses.Models;
using Expenses.Serializers;
using Microsoft.IdentityModel.Tokens;

namespace Expenses.Services;

public class TokenService
{
    private readonly Configuration _config;

    public TokenService(Configuration config)
    {
        _config = config;
    }
    
    public string GenerateToken(UserModel user)
    {
        // Gera uma instãncia do JTW class
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_config.GetPrivateKey());
        
        // credenciais para criar o token
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaimsIdentity(user),
            SigningCredentials = credentials,
            Issuer = _config.GetIssuer(),
            Audience = _config.GetAudience(),
            NotBefore = ConvertTimeZone.Convert(DateTime.UtcNow),
            Expires = ConvertTimeZone.Convert(DateTime.UtcNow,_config.GetExpires()),
        };
        
        // Gera o token
        var token = handler.CreateToken(tokenDescriptor);
        
        // Gera uma string do token
        return handler.WriteToken(token);
    }
    public string GenerateRefreshToken(UserModel user)
    {
        // Gera uma instãncia do JTW class
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_config.GetPrivateKey());
        
        // credenciais para criar o token
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = GenerateClaimsIdentityRefresh(user),
            SigningCredentials = credentials,
            Issuer = _config.GetIssuer(),
            Audience = _config.GetAudience(),
            NotBefore = ConvertTimeZone.Convert(DateTime.UtcNow),
            Expires = ConvertTimeZone.Convert(DateTime.UtcNow, _config.GetExpiresRefresh()),
        };
        
        // Gera o token
        var token = handler.CreateToken(tokenDescriptor);
        
        // Gera uma string do token
        return handler.WriteToken(token);
    }
    public string GenerateTokenInvalid()
    {
        // Gera uma instãncia do JTW class
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_config.GetPrivateKey());
        
        // credenciais para criar o token
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(_config.GetExpires()),
        };
        
        // Gera o token
        var token = handler.CreateToken(tokenDescriptor);
        
        // Gera uma string do token
        return handler.WriteToken(token);
    }
    public string GenerateRefreshTokenInvalid()
    {
        // Gera uma instãncia do JTW class
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_config.GetPrivateKey());
        
        // credenciais para criar o token
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(_config.GetExpiresRefresh()),
        };
        
        // Gera o token
        var token = handler.CreateToken(tokenDescriptor);
        
        // Gera uma string do token
        return handler.WriteToken(token);
    }
    public static ClaimsIdentity GenerateClaimsIdentity(UserModel user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Name, user.UserId.ToString()));
        foreach (var role in user.Roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));
        
        return ci;
    }
    
    public static ClaimsIdentity GenerateClaimsIdentityRefresh(UserModel user)
    {
        var ci = new ClaimsIdentity();
        ci.AddClaim(new Claim(ClaimTypes.Name, user.UserId.ToString()));
        
        return ci;
    }
    
    public async Task<(bool isValid, string UserId)> ValidateToken(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            return (false, string.Empty);
        
        var tokenParams = TokenHelpers.GetTokenValidationParameters(_config);

        var handler = new JwtSecurityTokenHandler();

        var validTokenResult = await handler.ValidateTokenAsync(token, tokenParams);
        
        if (!validTokenResult.IsValid)
            return (false, string.Empty);
        
        var userId = validTokenResult.Claims.FirstOrDefault(c => c.Key == ClaimTypes.Name).Value as string;

        return (true, userId);
    }
}