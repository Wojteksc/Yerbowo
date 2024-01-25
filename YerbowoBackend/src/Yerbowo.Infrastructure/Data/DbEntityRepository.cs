namespace Yerbowo.Infrastructure.Data;

public class DbEntityRepository<TEntity> : IDbEntityRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly YerbowoContext _db;

    protected DbSet<TEntity> _entities => _db.Set<TEntity>();

    protected IQueryable<TEntity> _entitiesNotRemoved => _db.Set<TEntity>().Where(c => !c.IsRemoved).AsQueryable();

    public DbEntityRepository(YerbowoContext db)
    {
        _db = db;
    }

    public virtual async Task<TEntity> GetAsync(int id)
    {
        return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
    {
        IQueryable<TEntity> resultWithEagerLoading = func(_entitiesNotRemoved);

        return await resultWithEagerLoading.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _entitiesNotRemoved.AsNoTracking().ToListAsync();
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);

        return await SaveAllAsync();
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Update(entity);

        return await SaveAllAsync();
    }

    public virtual async Task<bool> RemoveAsync(TEntity entity)
    {
        entity.IsRemoved = true;

        return await SaveAllAsync();
    }

    public virtual async Task<bool> SaveAllAsync()
    {
        return await _db.SaveChangesAsync(default) > 0;
    }

    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _entities.AnyAsync(x => x.Id == id);
    }
}