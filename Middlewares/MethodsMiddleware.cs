namespace Expenses.Middlewares;

public class MethodsMiddleware
{
    private RequestDelegate _next { get; set; }
    
    public MethodsMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method.ToUpper();
        var allowedMethods = AllowedHttpMethods.Methods;

        if (!allowedMethods.Contains(method))
        {
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            await context.Response.WriteAsync($"Metodo {method} nao permitido.");
            return;
        }

        await _next(context);
    }
}