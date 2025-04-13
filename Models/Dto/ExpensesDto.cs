namespace Expenses.Models.Dto;

public record ExpensesDto(Guid Id, string NameExpense, decimal AmountExpense, string DescriptionExpense, string CategoryExpense, DateTime DateExpense);