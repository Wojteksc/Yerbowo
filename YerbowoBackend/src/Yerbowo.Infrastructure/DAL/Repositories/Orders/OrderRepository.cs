namespace Yerbowo.Infrastructure.DAL.Repositories.Orders;

public class OrderRepository : DbEntityRepository<Order>, IOrderRepository
{
    public OrderRepository(YerbowoContext db) : base(db)
    {
    }

    public override async Task<Order> GetAsync(Guid id)
    {
        return await _entitiesNotRemoved
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Address)
            .Include(x => x.OrderItems)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Subcategory)
            .ThenInclude(x => x.Category)
            .SingleAsync(x => x.Id == id);
    }

    public async Task<ICollection<Order>> GetByUserAsync(Guid userId)
    {
        //Do testów:

        var query = _entitiesNotRemoved
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id);

        var sql = query.ToQueryString();

        return await _entitiesNotRemoved
            .Include(x => x.OrderItems)
            .ThenInclude(y => (y as OrderItem).Product)
            .Where(t => t.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ThenByDescending(x => x.Id)
            .AsNoTracking()
            .ToListAsync();
    }
}