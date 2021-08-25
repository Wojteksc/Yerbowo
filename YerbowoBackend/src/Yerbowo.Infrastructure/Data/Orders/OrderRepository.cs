using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yerbowo.Domain.Orders;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.Subcategories;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.SeedWork;

namespace Yerbowo.Infrastructure.Data.Orders
{
    public class OrderRepository : DbEntityRepository<Order>, IOrderRepository
    {
        public OrderRepository(YerbowoContext db) : base(db)
        {
        }

        public override async Task<Order> GetAsync(int id)
        {
            return await _entitiesNotRemoved
                .Include(x => x.Address)
                .Include(x => x.OrderItems)
                .ThenInclude(oi => (oi as OrderItem).Product)
                .ThenInclude(p => (p as Product).Subcategory)
                .ThenInclude(s => (s as Subcategory).Category)
                .AsNoTracking()
                .SingleAsync(x => x.Id == id);
        }

        public async Task<ICollection<Order>> GetByUserAsync(int userId)
        {
            return await _entitiesNotRemoved
                .Include(x => x.OrderItems)
                .ThenInclude(y => (y as OrderItem).Product)
                .Where(t => t.User.Id == userId)
                .OrderByDescending(x => x.Id)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
