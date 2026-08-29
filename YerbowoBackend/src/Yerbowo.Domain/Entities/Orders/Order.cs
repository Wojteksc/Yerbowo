namespace Yerbowo.Domain.Entities.Orders;

public class Order : BaseEntity
{
    public Guid UserId { get; protected set; }

    public User User { get; protected set; }

    public Guid AddressId { get; protected set; }

    public Address Address { get; protected set; }

    public OrderStatus OrderStatus { get; protected set; }

    public decimal TotalCost { get; protected set; }

    public string Comment { get; protected set; }

    public List<OrderItem> OrderItems { get; protected set; }

    private Order() { }

    public Order(
        Guid id,
        Guid userId, 
        Guid addressId, 
        OrderStatus orderStatus,
        decimal totalCost, 
        string comment, 
        List<OrderItem> orderItems)
    {
        Against.Default(id, nameof(id));
        Against.Default(userId, nameof(userId));
        Against.Default(addressId, nameof(addressId));
        Against.Negative(totalCost, nameof(totalCost));
        Against.Null(orderItems, nameof(orderItems));

        Id = id;
        UserId = userId;
        AddressId = addressId;
        TotalCost = totalCost;
        OrderStatus = orderStatus;
        Comment = comment;
        OrderItems = orderItems;
    }

    public decimal GetTotal()
    {
        return OrderItems.Sum(x => x.GetTotal());
    }
}