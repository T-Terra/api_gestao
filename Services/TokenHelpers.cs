using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses.Models;
using Microsoft.IdentityModel.Tokens;

namespace Expenses.Services;

public static class TokenHelpers
{
    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
    {
        //options.RequireHttpsMetadata = false; // deixe true em produção
        //options.SaveToken = true;
        return new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:SecretKey"] ?? string.Empty)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = configuration["JwtSettings:Issuer"],
            ValidAudience = configuration["JwtSettings:Audience"],
            ClockSkew = TimeSpan.Zero // sem tolerância de tempo para expiração
        };
    }

    public static async Task<(bool isValid, Guid? UserId)> ValidateToken(string token)
    {
        if(string.IsNullOrWhiteSpace(token))
            return (false, Guid.Empty);
        
        var tokenParams = GetTokenValidationParameters(_configuration);

        var validTokenResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenParams);
        
        if (!validTokenResult.IsValid)
            return (false, Guid.Empty);
        
        var userId = validTokenResult.Claims.FirstOrDefault(c => c.Key == ClaimTypes.Name).Value as Guid?;
        
        return (true, userId);
    }
}