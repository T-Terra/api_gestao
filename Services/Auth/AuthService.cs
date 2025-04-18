using System.Security.Cryptography;
using System.Text;
using Expenses.Config;
using Expenses.Data;
using Expenses.Models.Requests;
using Expenses.Serializers;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Services.Auth;

public class AuthService
{
    private readonly ExpenseContext _context;
    private readonly TokenService _tokenService;
    private readonly Configuration _configuration;
    
    public AuthService(ExpenseContext context, TokenService tokenService, Configuration configuration)
    {
        _context = context;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<IResult> Login(HttpContext http, UserRequest req, CancellationToken ct)
    {
        var bytes = Encoding.UTF8.GetBytes(req.Password);
        var hash = SHA256.HashData(bytes);

        var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
            
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == req.Email, ct);
            
        if (user == null)
            return Results.Unauthorized();
            
        if (user.Password != hashString)
            return Results.Unauthorized();
            
        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(user);
            
        var tokenDateTime = ConvertTimeZone.Convert(DateTimeOffset.UtcNow, _configuration.GetExpires());
        var refreshDateTime = ConvertTimeZone.Convert(DateTimeOffset.UtcNow, _configuration.GetExpiresRefresh());

        var cookiesOptionsToken = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Expires = tokenDateTime,
        };
            
        var cookiesOptionsRefresh = new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            Expires = refreshDateTime,
        };
            
        http.Response.Cookies.Append("token", token, cookiesOptionsToken);
        http.Response.Cookies.Append("refreshtoken", refreshToken, cookiesOptionsRefresh);
            
        return Results.Ok(new { username = user.Username, message = "Login feito com sucesso." });
    }
}