namespace Expenses.Models.Dto;

public record ExpensesDtoWithoutName(Guid Id, string NameExpense, decimal AmountExpense, string DescriptionExpense, DateTime DateExpense, string? CategoryId);