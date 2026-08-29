namespace Yerbowo.Domain.Entities.Orders;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; protected set; }
    public Order Order { get; protected set; }
    public Guid ProductId { get; protected set; }
    public Product Product { get; protected set; }
    public int Quantity { get; protected set; }
    public decimal Price { get; protected set; }

    private OrderItem() { }

    public OrderItem(Guid id, Guid productId, int quantity, decimal price)
    {
        Against.Default(id, nameof(id));
        Against.Default(productId, nameof(productId));
        Against.NegativeOrZero(quantity, nameof(quantity));
        Against.NegativeOrZero(price, nameof(price));
        
        Id = id;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public decimal GetTotal()
    {
        return Quantity * Price;
    }
}