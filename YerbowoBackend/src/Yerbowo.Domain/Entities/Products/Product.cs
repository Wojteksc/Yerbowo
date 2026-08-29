namespace Yerbowo.Domain.Entities.Products;

public class Product : BaseEntity
{
    public Guid SubcategoryId { get; protected set; }

    public string Code { get; protected set; }
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public decimal Price { get; protected set; }
    public decimal OldPrice { get; protected set; }
    public int Stock { get; protected set; }
    public ProductState State { get; protected set; }
    public string Image { get; protected set; }
    public string Slug { get; protected set; }

    public Subcategory Subcategory { get; protected set; }

    private Product() { }

    public Product(
        Guid id,
        Guid subcategoryId, 
        string code, 
        string name,
        string description, 
        decimal price, 
        decimal oldPrice,
        int stock, 
        ProductState state, 
        string image)

    {
        Against.Default(id, nameof(id));
        Against.Default(subcategoryId, nameof(subcategoryId));
        Against.NullOrEmpty(code, nameof(code));
        Against.NullOrEmpty(name, nameof(name));
        Against.NullOrEmpty(description, nameof(description));
        Against.Negative(price, nameof(price));
        Against.Negative(stock, nameof(stock));
        Against.NullOrEmpty(image, nameof(image));

        Id = id;
        SubcategoryId = subcategoryId;
        Code = code;
        Name = name;
        Description = description;
        Price = price;
        SetOldPrice(oldPrice);
        OldPrice = oldPrice;
        Stock = stock;
        SetState(state);
        Image = image;
        Slug = name.ToSlug();
    }

    public void SetState(ProductState state)
    {
        Against.Null(state, nameof(state));
        State = state;
    }

    public void SetOldPrice(decimal oldPrice)
    {
        Against.Negative(oldPrice, nameof(oldPrice));
        OldPrice = oldPrice;
    }
}