namespace Yerbowo.Domain.Entities.Categories;

public class Category : BaseEntity
{
    public string Name { get; protected set; }
    public string Slug { get; protected set; }
    public string Description { get; protected set; }
    public string Image { get; protected set; }

    public ICollection<Subcategory> Subcategories { get; protected set; }

    private Category() { }

    public Category(Guid id, string name, string description, string image)
    {
        Against.Default(id, nameof(id));
        Against.NullOrEmpty(name, nameof(name));
        Against.NullOrEmpty(description, nameof(description));
        Against.NullOrEmpty(image, nameof(image));
        
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        Slug = name.ToSlug();   
    }
}