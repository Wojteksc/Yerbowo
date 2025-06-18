namespace Yerbowo.Infrastructure.DAL.Repositories.Products;

public class ProductRepository : DbEntityRepository<Product>, IProductRepository
{
    public ProductRepository(YerbowoContext db) : base(db)
    { }

    public async Task<Product> GetAsync(string slug)
    {
        return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<Product> GetWithCategoryAsync(int productId)
    {
        IQueryable<Product> resultWithEagerLoading = _entitiesNotRemoved
            .Include(x => x.Subcategory)
            .ThenInclude(x => x.Category);

        return await resultWithEagerLoading.SingleOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<Product> GetWithCategoryAsync(string slug)
    {
        IQueryable<Product> resultWithEagerLoading = _entitiesNotRemoved
            .Include(x => x.Subcategory)
            .ThenInclude(x => x.Category);

        return await resultWithEagerLoading.SingleOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<bool> ExistsAsync(string slug)
    {
        return await _entitiesNotRemoved.AnyAsync(x => x.Slug == slug);
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

        if (!string.IsNullOrEmpty(subcategory))
        {
            products = products.Where(p => p.Subcategory.Slug == subcategory);
        }

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(p => p.Subcategory.Category.Slug == category);
        }

        products = products.AsNoTracking().OrderBy(x => Guid.NewGuid());

        return await PagedList<Product>.CreateAsync(products, pageNumber, pageSize);
    }
}