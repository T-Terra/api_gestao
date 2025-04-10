using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses.Config;
using Expenses.Models;
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
            Expires = DateTime.UtcNow.AddHours(2),
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
}