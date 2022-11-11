namespace Yerbowo.Infrastructure.Data.Orders;

public interface IOrderRepository : IEntityRepository<Order>
{
    Task<ICollection<Order>> GetByUserAsync(int userId);
}