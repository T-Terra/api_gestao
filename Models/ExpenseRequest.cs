namespace Expenses.Models;

public record ExpenseRequest(string NameExpense, decimal AmountExpense, string DescriptionExpense, string CategoryId, Guid UserId);