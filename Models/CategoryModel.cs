using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expenses.Models;

public class CategoryModel
{
    [Key]
    public Guid CategoryId { get; init; }
    public string NameCategory { get; private set; }
    public string DescriptionCategory { get; private set; }
    public DateTime DateCreated { get; init; }
    [ForeignKey("User")]
    public Guid? UserId { get; set; } // 🔗 chave estrangeira
    public UserModel? User { get; set; } // relação de navegação
    public List<ExpensesModel> Expenses { get; set; } = new();
    
    public CategoryModel() {}

    public CategoryModel(string nameCategory, string descriptionCategory, Guid userId)
    {
        CategoryId = Guid.NewGuid();
        NameCategory = nameCategory;
        DescriptionCategory = descriptionCategory;
        UserId = userId;
        DateCreated = DateTime.UtcNow;
    }

    public void SetCategory(string nameCategory, string descriptionCategory)
    {
        NameCategory = nameCategory;
        DescriptionCategory = descriptionCategory;
    }
}