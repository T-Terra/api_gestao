namespace Expenses.Models;

public record ExpenseRequest(string NameExpense, int AmountExpense, string DescriptionExpense, string CategoryExpense);