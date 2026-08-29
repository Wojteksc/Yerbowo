namespace Yerbowo.Application.Abstractions.Repositories;

public interface IDbEntityRepository<TEntity>
{
    Task<TEntity> GetAsync(Guid id);
    Task<TEntity> GetAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task<bool> SaveAllAsync();
    Task<bool> ExistsAsync(Guid id);
}