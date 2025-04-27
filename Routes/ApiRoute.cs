using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Expenses.Config;
using Expenses.Data;
using Expenses.Models;
using Expenses.Models.Dto;
using Expenses.Models.Requests;
using Expenses.Routes.AuthRoutes;
using Expenses.Serializers;
using Expenses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Routes;

public static class ApiRoute
{
    public static void ApiRoutes(this WebApplication app)
    {
        var route = app.MapGroup("api");
        
        //Auth
        route.MapAuthRoutes();
        
        // Expenses
        route.MapPost("add", [Authorize] async (HttpContext http, ExpenseRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Results.Conflict();
            
            // cria uma despesa
            var userId = Guid.Parse(userIdStr);
            var expense = new ExpensesModel(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense, userId, req.CategoryId);
            // adiciona no banco de dados
            await context.AddAsync(expense, ct);
            // Faz o commit no banco de dados
           await context.SaveChangesAsync(ct);
            
            return Results.Created("/api/add", new ExpensesDto(
                expense.Id, 
                expense.NameExpense, 
                expense.AmountExpense, 
                expense.DescriptionExpense, 
                expense.CategoryExpense,
                expense.CategoryId,
                expense.DateExpense));
        });

        route.MapGet("list", [Authorize] async (HttpContext http, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Results.Conflict();
            
            var userId = Guid.Parse(userIdStr);
            var expenses = await context.Expenses
                .Where(exp => exp.UserId == userId)
                .Select(c => new ExpensesDto(
                    c.Id,
                    c.NameExpense,
                    c.AmountExpense,
                    c.DescriptionExpense,
                    c.CategoryExpense,
                    c.CategoryId,
                    c.DateExpense))
                .ToListAsync(ct);
            return Results.Ok(expenses);
        });

        route.MapPut("update/{id:guid}", [Authorize] async (Guid id, ExpenseRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id, ct);
            
            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }
            
            expense.ChangeValues(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense, req.CategoryId);
            await context.SaveChangesAsync(ct);
            
            return Results.Ok(new ExpensesDto(
                expense.Id, 
                expense.NameExpense, 
                expense.AmountExpense, 
                expense.DescriptionExpense, 
                expense.CategoryExpense,
                expense.CategoryId,
                expense.DateExpense));
        });

        route.MapDelete("delete/{id:guid}", [Authorize] async (Guid id, ExpenseContext context, CancellationToken ct) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }

            context.Expenses.Remove(expense);
            await context.SaveChangesAsync(ct);

            return Results.Ok(new ExpensesDto(
                expense.Id, 
                expense.NameExpense, 
                expense.AmountExpense, 
                expense.DescriptionExpense, 
                expense.CategoryExpense,
                expense.CategoryId,
                expense.DateExpense));
        });

        route.MapGet("expenses/total", [Authorize] async (HttpContext http, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Results.Conflict();
            
            var userId = Guid.Parse(userIdStr);
            
            var total = await context.Expenses
                .Where(exp => exp.UserId == userId)    
                .SumAsync(e => e.AmountExpense, ct);
            return Results.Ok(new {total});
        });
        
        // Revenues
        route.MapPost("revenue", [Authorize] async (HttpContext http, RevenueDto req, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Results.Conflict();
            
            var userId = Guid.Parse(userIdStr);
            
            var revenue = new RevenueModel(req.AmountRevenue, userId);
            await context.AddAsync(revenue, ct);
            await context.SaveChangesAsync(ct);
            
            return Results.Created("/api/revenue", new RevenueDto(revenue.AmountRevenue, revenue.DateRevenue));
        });
        
        route.MapGet("revenue", [Authorize] async (HttpContext http, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            if (string.IsNullOrWhiteSpace(userIdStr))
                return Results.Conflict();
            
            var userId = Guid.Parse(userIdStr);
            
            var revenue = await context.Revenues
                .Where(e => e.UserId == userId)
                .Select(c => new RevenueDto(
                    c.AmountRevenue,
                    c.DateRevenue))
                .ToListAsync(ct);
            
            return Results.Ok(revenue);
        });
        
        // Category
        route.MapPost("category", [Authorize] async (HttpContext http, CategoryRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            var guid = Guid.TryParse(userIdStr, out Guid userId);

            if (guid == false)
            {
                return Results.Conflict();
            }
            
            var category = new CategoryModel(req.NameCategory, req.DescriptionCategory, userId);
            await context.AddAsync(category, ct);
            await context.SaveChangesAsync(ct);
            
            return Results.Created("/api/category", new CategoryDto(category.CategoryId, category.NameCategory, category.DescriptionCategory, category.DateCreated));
        });
        
        route.MapPut("category/{id:guid}", [Authorize] async (Guid id, CategoryRequest req, ExpenseContext context, CancellationToken ct) =>
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
            
            if (category == null)
                return Results.NotFound();
            
            category.SetCategory(req.NameCategory, req.DescriptionCategory);
            await context.SaveChangesAsync(ct);
            
            return Results.Ok(new CategoryDto(
                id, 
                category.NameCategory, 
                category.DescriptionCategory, 
                category.DateCreated
                ));
        });

        route.MapGet("category", [Authorize] async (HttpContext http, ExpenseContext context, CancellationToken ct) =>
        {
            var userIdStr = http.User.FindFirst(ClaimTypes.Name)?.Value;
            
            var category = await context.Categories
                .Where(c => c.UserId.ToString() == userIdStr)
                .Select(c => new CategoryDto(
                    c.CategoryId,
                    c.NameCategory, 
                    c.DescriptionCategory, 
                    c.DateCreated))
                .ToListAsync(ct);
            
            return Results.Ok(category);
        });

        route.MapDelete("category/{id:guid}", [Authorize] async (Guid id, ExpenseContext context, CancellationToken ct) =>
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id, ct);
            
            if (category == null)
            {
                return Results.NotFound($"Categoria com ID {id} não encontrada.");
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync(ct);

            return Results.Ok(new CategoryDto(category.CategoryId, category.NameCategory, category.DescriptionCategory, category.DateCreated));
            
        });
    }
}