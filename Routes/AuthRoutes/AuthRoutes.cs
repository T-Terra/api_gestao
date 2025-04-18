using Expenses.Models.Requests;
using Expenses.Services.Auth;
using Microsoft.AspNetCore.Authorization;

namespace Expenses.Routes.AuthRoutes;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this RouteGroupBuilder route)
    {
        route.MapPost("login",
            async (
                HttpContext http, 
                UserRequest req, 
                AuthService authService, 
                CancellationToken ct) => 
                await authService.Login(http, req, ct));
        
        route.MapPost("register", async (
            UserRequest req, 
            AuthService authService, 
            CancellationToken ct) => await authService.Register(req, ct));

        route.MapPost("refresh-token", async (
            HttpContext http, AuthService authService, CancellationToken ct) => await authService.RefreshToken(http, ct));
        
        route.MapGet("authcheck", [Authorize] async (AuthService authService) => await authService.AuthCheck());
        route.MapPost("logout", [Authorize] async (HttpContext http, AuthService authService) => await authService.Logout(http));
    }
}