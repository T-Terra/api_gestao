using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models;

public class RevenueModel
{
    public Guid Id { get; init; }
    public decimal AmountRevenue { get; private set; }
    public DateTime DateRevenue { get; private set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public UserModel User { get; set; }
    
    public RevenueModel() {}
    public RevenueModel(decimal amountRevenue, Guid userId)
    {
        Id = Guid.NewGuid();
        AmountRevenue = amountRevenue;
        UserId = userId;
        DateRevenue = DateTime.UtcNow;
    }

    public void SetAmountRevenue(decimal amountRevenue)
    {
        AmountRevenue = amountRevenue;
    }
}