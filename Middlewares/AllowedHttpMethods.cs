namespace Expenses.Middlewares;

public static class AllowedHttpMethods
{
    public static readonly string[] Methods = new[] { "PUT", "POST", "DELETE", "GET", "OPTIONS" };
}