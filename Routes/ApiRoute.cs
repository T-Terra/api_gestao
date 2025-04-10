using Expenses.Data;
using Expenses.Models;
using Expenses.Models.Dto;
using Expenses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Routes;

public static class ApiRoute
{
    public static void ApiRoutes(this WebApplication app)
    {
        var route = app.MapGroup("api");
        
        //Token
        route.MapGet("gentoken", [Authorize] (TokenService service) 
            => service.GenerateToken(new UserModel("Gabriel", "123", new[]
            {
                "viewer", "premium"
            })));
        
        // Expenses
        route.MapPost("add", async (ExpenseRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            // cria uma despesa
            var expense = new ExpensesModel(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense);
            // adiciona no banco de dados
            await context.AddAsync(expense, ct);
            // Faz o commit no banco de dados
            await context.SaveChangesAsync(ct);
            
            return Results.Created("/api/add", expense);
        });

        route.MapGet("list", async (ExpenseContext context, CancellationToken ct) =>
        {
            var expenses = await context.Expenses.ToListAsync(ct);
            return Results.Ok(expenses);
        });

        route.MapPut("update/{id:guid}", async (Guid id, ExpenseRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id, ct);
            
            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }
            
            expense.ChangeValues(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense);
            await context.SaveChangesAsync(ct);
            
            return Results.Ok(expense);
        });

        route.MapDelete("delete/{id:guid}", async (Guid id, ExpenseContext context, CancellationToken ct) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }

            context.Expenses.Remove(expense);
            await context.SaveChangesAsync(ct);

            return Results.Ok(expense);
        });

        route.MapGet("expenses/total", async (ExpenseContext context, CancellationToken ct) =>
        {
            var total = await context.Expenses.SumAsync(e => e.AmountExpense, ct);
            return Results.Ok(new { total});
        });
        
        // Revenues
        route.MapPost("revenue", async (RevenueDto req, ExpenseContext context, CancellationToken ct) =>
        {
            var revenue = new RevenueModel(req.AmountRevenue);
            await context.AddAsync(revenue, ct);
            await context.SaveChangesAsync(ct);
            return Results.Created("/api/revenue", revenue);
        });
        
        route.MapGet("revenue", async (ExpenseContext context, CancellationToken ct) =>
        {
            var revenue = await context.Revenues.ToListAsync(ct);
            return Results.Ok(revenue);
        });
        
        // Category
        route.MapPost("category", async (CategoryDto req, ExpenseContext context, CancellationToken ct) =>
        {
            var category = new CategoryModel(req.NameCategory, req.DescriptionCategory);
            
            await context.AddAsync(category, ct);
            await context.SaveChangesAsync(ct);
            return Results.Created("/api/category", category);
        });

        route.MapGet("category", async (ExpenseContext context, CancellationToken ct) =>
        {
            var category = await context.Categories.ToListAsync(ct);
            return Results.Ok(category);
        });

        route.MapDelete("category/{id:guid}", async (Guid id, ExpenseContext context, CancellationToken ct) =>
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
            
            if (category == null)
            {
                return Results.NotFound($"Categoria com ID {id} não encontrada.");
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync(ct);

            return Results.Ok(category);
            
        });
    }
}