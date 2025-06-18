namespace Yerbowo.Infrastructure.DAL.Repositories;

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
        => await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
    {
        IQueryable<TEntity> resultWithEagerLoading = func(_entitiesNotRemoved);

        return await resultWithEagerLoading.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() 
        => await _entitiesNotRemoved.AsNoTracking().ToListAsync();

    public virtual async Task<bool> ExistsAsync(int id)
        => await _entities.AnyAsync(x => x.Id == id);

    public virtual async Task AddAsync(TEntity entity) 
        => await _entities.AddAsync(entity);

    public virtual Task UpdateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _entities.Update(entity);

        return Task.CompletedTask;
    }

    public virtual Task RemoveAsync(TEntity entity)
    {
        entity.IsRemoved = true;

        return Task.CompletedTask;
    }

    public virtual async Task<bool> SaveAllAsync() 
        => await _db.SaveChangesAsync(default) > 0;
}