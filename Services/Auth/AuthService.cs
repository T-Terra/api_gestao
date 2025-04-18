using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expenses.Config;
using Expenses.Data;
using Expenses.Models;
using Expenses.Models.Dto;
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

    public async Task<IResult> Register(UserRequest req, CancellationToken ct)
    {
        var userExists = await _context.Users.FirstOrDefaultAsync(user => user.Email == req.Email, ct);
            
        if (userExists != null)
            return Results.Conflict(new { Message = $"User {req.Email} already exists."});
            
        var bytesPassword = Encoding.UTF8.GetBytes(req.Password);
        var hash = SHA256.HashData(bytesPassword);

        var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
            
        var user = new UserModel(req.Username, hashString, req.Email);
            
        await _context.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
            
        return Results.Created($"/api/users/{user.UserId}", new UserDto(user.Username, user.Email));
    }

    public async Task<IResult> RefreshToken(HttpContext http, CancellationToken ct)
    {
        var tokenHeader = http.Request.Cookies["token"] ?? string.Empty;
        var refreshHeader = http.Request.Cookies["refreshtoken"] ?? string.Empty;
            
        if(!string.IsNullOrEmpty(tokenHeader))
            return Results.NoContent();
            
        if(string.IsNullOrWhiteSpace(refreshHeader))
            return Results.BadRequest();
            
        var isValidatedResult = await _tokenService.ValidateToken(refreshHeader);
            
        var userId = isValidatedResult.UserId;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId, ct);
            
        if (user == null)
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
            
        return Results.Ok(new { username = user.Username, message = "Sessão atualizada com sucesso." });
    }

    public async Task<IResult> AuthCheck()
    {
        return Results.NoContent();
    }
    
    public async Task<IResult> Logout(HttpContext http)
    {
        var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
        if (string.IsNullOrWhiteSpace(userIdStr))
            return Results.Unauthorized();
                
        var token = _tokenService.GenerateTokenInvalid();
        var refreshToken = _tokenService.GenerateRefreshTokenInvalid();
                
        var cookiesOptionsToken = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
        };
                
        var cookiesOptionsRefresh = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(-1),
        };
                
        http.Response.Cookies.Append("token", token, cookiesOptionsToken);
        http.Response.Cookies.Append("refreshtoken", refreshToken, cookiesOptionsRefresh);
            
        return Results.Ok(new { message = "Logout feito com sucesso." });
    }
}