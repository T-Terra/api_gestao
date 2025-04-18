using Expenses.Models.Requests;
using Expenses.Services.Auth;

namespace Expenses.Routes.AuthRoutes;

public static class AuthRoutes
{
    public static void MapAuthRoutes(this RouteGroupBuilder route)
    {
        route.MapPost("login",
            async (
                HttpContext http, 
                UserRequest req, 
                AuthService authService , 
                CancellationToken ct) => 
                await authService.Login(http, req, ct));
    }
}