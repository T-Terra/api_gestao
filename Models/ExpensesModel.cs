using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models;

public class ExpensesModel
{
    // Variables
    public Guid Id { get; init; }
    public string NameExpense { get; private set; }
    public decimal AmountExpense { get; private set; }
    public string? DescriptionExpense { get; private set; }
    public DateTime DateExpense { get; private set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public UserModel User { get; set; }
    [ForeignKey("Category")]
    public Guid? CategoryId { get; set; }
    public CategoryModel? Categories { get; set; }
    
    // Construtor
    
    public ExpensesModel() { }
    public ExpensesModel(string nameExpense, decimal amountExpense, string descriptionExpense, Guid userId)
    {
        Id = Guid.NewGuid();
        NameExpense = nameExpense;
        AmountExpense = amountExpense;
        DescriptionExpense = descriptionExpense;
        UserId = userId;
        DateExpense = DateTime.UtcNow;
    }

    public void ChangeValues(string nameExpense, decimal amountExpense, string descriptionExpense)
    {
        NameExpense = nameExpense;
        AmountExpense = amountExpense;
        DescriptionExpense = descriptionExpense;
    }

    public void SetCategory(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}