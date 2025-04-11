using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Expenses.Config;
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

    public static TokenValidationParameters GetTokenValidationParameters(Configuration configuration)
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetPrivateKey())),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = configuration.GetIssuer(),
            ValidAudience = configuration.GetAudience(),
            ClockSkew = TimeSpan.Zero // sem tolerância de tempo para expiração
        };
    }
}