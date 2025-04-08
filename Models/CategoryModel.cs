namespace Expenses.Models;

public class CategoryModel
{
    public Guid Id { get; init; }
    public string NameCategory { get; private set; }
    public string DescriptionCategory { get; private set; }
    public DateTime DateCreated { get; init; }
    
    public CategoryModel() {}

    public CategoryModel(string nameCategory, string descriptionCategory)
    {
        Id = Guid.NewGuid();
        NameCategory = nameCategory;
        DescriptionCategory = descriptionCategory;
        DateCreated = DateTime.UtcNow;
    }

    public void SetCategory(string nameCategory, string descriptionCategory)
    {
        NameCategory = nameCategory;
        DescriptionCategory = descriptionCategory;
    }
}