using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using Yerbowo.Domain.Addresses;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Domain.Users;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Orders
{
    public class Order : BaseEntity
    {
        public int UserId { get; protected set; }

        public User User { get; protected set; }

        public int AddressId { get; protected set; }

        public Address Address { get; protected set; }

        public OrderStatus OrderStatus { get; protected set; }

        public decimal TotalCost { get; protected set; }

        public string Comment { get; protected set; }

        public List<OrderItem> OrderItems { get; protected set; }

        private Order() {}

        public Order(int userId, int addressId, OrderStatus orderStatus,
            decimal totalCost, string comment, List<OrderItem> orderItems)
        {
            Against.NegativeOrZero(userId, nameof(userId));
            Against.NegativeOrZero(addressId, nameof(addressId));
            Against.Negative(totalCost, nameof(totalCost));
            Against.Null(orderItems, nameof(orderItems));

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
}
