namespace Expenses.Models.Dto;

public record ExpensesDto(Guid Id, string NameExpense, decimal AmountExpense, string DescriptionExpense, DateTime DateExpense, string? CategoryName);