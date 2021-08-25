using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yerbowo.Domain.Products;
using Yerbowo.Infrastructure.Context;
using Yerbowo.Infrastructure.Data.SeedWork;
using Yerbowo.Infrastructure.Helpers;

namespace Yerbowo.Infrastructure.Data.Products
{
    public class ProductRepository : DbEntityRepository<Product>, IProductRepository
    {
        public ProductRepository(YerbowoContext db) : base(db)
        {
        }

        public async Task<Product> GetAsync(string slug)
        {
            return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Slug == slug);
        }

        public async Task<Product> GetAsync(string slug, Func<IQueryable<Product>, IQueryable<Product>> func)
        {
            IQueryable<Product> resultWithEagerLoading = func(_entitiesNotRemoved);

            return await resultWithEagerLoading.SingleOrDefaultAsync(e => e.Slug == slug);
        }

        public async Task<IEnumerable<Product>> BrowseRandomAsync(int quantity)
        {
            var products = await _entitiesNotRemoved
                .Include(x => x.Subcategory)
                .ThenInclude(x => x.Category)
                .Take(quantity)
                .OrderBy(x => Guid.NewGuid())
                .AsNoTracking()
                .ToListAsync();

            return products;
        }

        public async Task<PagedList<Product>> BrowseAsync(int pageNumber, int pageSize, string category, string subcategory)
        {
            var products = _entitiesNotRemoved
                .Include(s => s.Subcategory)
                .ThenInclude(c => c.Category)
                .AsQueryable();

            if(!string.IsNullOrEmpty(subcategory))
            {
                products = products.Where(p => p.Subcategory.Slug == subcategory);
            }

            if(!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Subcategory.Category.Slug == category);
            }

            products = products.AsNoTracking().OrderBy(x => Guid.NewGuid());

            return await PagedList<Product>.CreateAsync(products, pageNumber, pageSize);
        }
    }
}
