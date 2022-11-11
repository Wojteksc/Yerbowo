namespace Yerbowo.Infrastructure.Data.SeedWork;

public interface IEntityRepository<TEntity>
{
    Task<TEntity> GetAsync(int id);
    Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> RemoveAsync(TEntity entity);
    Task<bool> SaveAllAsync();
    Task<bool> ExistsAsync(int id);
}