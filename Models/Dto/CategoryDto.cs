namespace Expenses.Models.Dto;

public record CategoryDto(Guid CategoryId, string NameCategory, string DescriptionCategory, DateTime DateCreated);