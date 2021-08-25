using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Infrastructure.Context;

namespace Yerbowo.Infrastructure.Data.SeedWork
{
    public class DbEntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly YerbowoContext _db;

        protected DbSet<TEntity> _entities => _db.Set<TEntity>();

        protected IQueryable<TEntity> _entitiesNotRemoved => _db.Set<TEntity>().Where(c => !c.IsRemoved).AsQueryable();

        public DbEntityRepository(YerbowoContext db)
        {
            _db = db;
        }

        public async virtual Task<TEntity> GetAsync(int id)
        {
            return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Id == id);
        }

        //GetAsync(1, x => x.Include(x => x.Object).ThenInclude(x => x.Object))
        public async Task<TEntity> GetAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> func)
        {
            IQueryable<TEntity> resultWithEagerLoading = func(_entitiesNotRemoved);

            return await resultWithEagerLoading.FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entitiesNotRemoved.AsNoTracking();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entitiesNotRemoved.AsNoTracking().ToListAsync();
        }

        public async virtual Task<bool> AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);

            return await SaveAllAsync();
        }

        public async virtual Task<bool> RemoveAsync(TEntity entity)
        {
            entity.IsRemoved = true;

            return await SaveAllAsync();
        }

        public async virtual Task<bool> UpdateAsync(TEntity entity)
        {
            _entities.Update(entity);

            return await SaveAllAsync();
        }

        public async virtual Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public async virtual Task<bool> ExistsAsync(int id)
        {
            return await _entities.AnyAsync(x => x.Id == id);
        }
    }
}
