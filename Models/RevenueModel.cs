namespace Expenses.Models;

public class RevenueModel
{
    public Guid Id { get; init; }
    public float AmountRevenue { get; private set; }
    public DateTime DateRevenue { get; private set; }
    
    public RevenueModel() {}
    public RevenueModel(float amountRevenue)
    {
        Id = Guid.NewGuid();
        AmountRevenue = amountRevenue;
        DateRevenue = DateTime.UtcNow;
    }

    public void SetAmountRevenue(float amountRevenue)
    {
        AmountRevenue = amountRevenue;
    }
}