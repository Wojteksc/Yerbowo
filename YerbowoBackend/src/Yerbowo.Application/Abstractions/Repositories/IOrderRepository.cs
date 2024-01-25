namespace Yerbowo.Application.Abstractions.Repositories;

public interface IOrderRepository : IDbEntityRepository<Order>
{
    Task<ICollection<Order>> GetByUserAsync(int userId);
}