using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yerbowo.Infrastructure.Data.SeedWork
{
    public interface IEntityRepository<TEntity>
    {
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func);
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<bool> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> SaveAllAsync();
        Task<bool> ExistsAsync(int id);
    }
}
