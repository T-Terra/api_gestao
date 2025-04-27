namespace Expenses.Models.Dto;

public record ExpensesDto(Guid Id, string NameExpense, decimal AmountExpense, string DescriptionExpense, string CategoryExpense, Guid CategoryId , DateTime DateExpense);