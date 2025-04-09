namespace Expenses.Models;

public class ExpensesModel
{
    // Variables
    public Guid Id { get; init; }
    public string NameExpense { get; private set; }
    public decimal AmountExpense { get; private set; }
    public string? DescriptionExpense { get; private set; }
    public string? CategoryExpense { get; private set; }
    public DateTime DateExpense { get; private set; }
    
    // Construtor
    
    public ExpensesModel() { }
    public ExpensesModel(string nameExpense, decimal amountExpense, string descriptionExpense, string categoryExpense)
    {
        Id = Guid.NewGuid();
        NameExpense = nameExpense;
        AmountExpense = amountExpense;
        DescriptionExpense = descriptionExpense;
        CategoryExpense = categoryExpense;
        DateExpense = DateTime.UtcNow;
    }

    public void ChangeValues(string nameExpense, decimal amountExpense, string descriptionExpense, string categoryExpense)
    {
        NameExpense = nameExpense;
        AmountExpense = amountExpense;
        DescriptionExpense = descriptionExpense;
        CategoryExpense = categoryExpense;
    }
}