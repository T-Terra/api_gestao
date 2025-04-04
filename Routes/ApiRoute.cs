using Expenses.Data;
using Expenses.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Routes;

public static class ApiRoute
{
    public static void ApiRoutes(this WebApplication app)
    {
        var route = app.MapGroup("api");
        route.MapPost("add", async (ExpenseRequest req, ExpenseContext context) =>
        {
            // cria uma despesa
            var expense = new ExpensesModel(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense);
            // adiciona no banco de dados
            await context.AddAsync(expense);
            // Faz o commit no banco de dados
            await context.SaveChangesAsync();
            
            return Results.Created("/api/add", expense);
        });

        route.MapGet("list", async (ExpenseContext context) =>
        {
            var expenses = await context.Expenses.ToListAsync();
            return Results.Ok(expenses);
        });

        route.MapPut("update/{id:guid}", async (Guid id, ExpenseRequest req, ExpenseContext context) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
            
            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }
            
            expense.ChangeValues(req.NameExpense, req.AmountExpense, req.DescriptionExpense, req.CategoryExpense);
            await context.SaveChangesAsync();
            
            return Results.Ok(expense);
        });

        route.MapDelete("delete/{id:guid}", async (Guid id, ExpenseContext context) =>
        {
            var expense = await context.Expenses.FirstOrDefaultAsync(x => x.Id == id);

            if (expense == null)
            {
                return Results.NotFound($"Despesa com ID {id} não encontrada.");
            }

            context.Expenses.Remove(expense);
            await context.SaveChangesAsync();

            return Results.Ok(expense);
        });
    }
}