using Expenses.Models;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Data;

public class ExpenseContext: DbContext
{
    // define as tabelas do banco
    public DbSet<ExpensesModel> Expenses { get; set; }
    public DbSet<RevenueModel> Revenues { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connectionString = configuration.GetConnectionString("PostgreSQLConnection");
        optionsBuilder.UseNpgsql(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
}