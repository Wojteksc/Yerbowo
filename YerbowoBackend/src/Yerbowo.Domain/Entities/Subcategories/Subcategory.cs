namespace Yerbowo.Domain.Entities.Subcategories;

public class Subcategory : BaseEntity
{
    public Guid CategoryId { get; protected set; }
    public string Name { get; protected set; }
    public string Slug { get; protected set; }
    public string Description { get; protected set; }
    public string Image { get; protected set; }
    public Category Category { get; protected set; }
    public ICollection<Product> Products { get; protected set; }

    private Subcategory() { }

    public Subcategory(
        Guid id, 
        Guid categoryId, 
        string name, 
        string description, 
        string image)
    {
        Against.Default(id, nameof(id));
        Against.Default(categoryId, nameof(categoryId));
        Against.NullOrEmpty(name, nameof(name));
        Against.NullOrEmpty(description, nameof(description));
        Against.NullOrEmpty(image, nameof(image));
        
        Id = id;
        CategoryId = categoryId;
        Name = name;
        Description = description;
        Image = image;
        Slug = name.ToSlug();
    }

}