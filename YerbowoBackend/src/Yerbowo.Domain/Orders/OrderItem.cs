using Ardalis.GuardClauses;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.SeedWork;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Orders
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; protected set; }
        public Order Order { get; protected set; }
        public int ProductId { get; protected set; }
        public Product Product { get; protected set; }
        public int Quantity { get; protected set; }
        public decimal Price { get; protected set; }

        private OrderItem() {}

        public OrderItem(int productId, int quantity, decimal price)
        {
            Against.NegativeOrZero(productId, nameof(productId));
            Against.NegativeOrZero(quantity, nameof(quantity));
            Against.NegativeOrZero(price, nameof(price));

            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public decimal GetTotal()
        {
            return Quantity * Price;
        }
    }
}
