namespace Expenses.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseMethodsMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MethodsMiddleware>();
    }
}