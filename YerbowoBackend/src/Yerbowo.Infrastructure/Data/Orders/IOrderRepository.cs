using System.Collections.Generic;
using System.Threading.Tasks;
using Yerbowo.Domain.Orders;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Orders
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
        Task<ICollection<Order>> GetByUserAsync(int userId);
    }
}
