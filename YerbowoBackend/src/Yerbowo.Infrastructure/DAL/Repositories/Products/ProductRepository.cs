namespace Yerbowo.Infrastructure.DAL.Repositories.Products;

public class ProductRepository : DbEntityRepository<Product>, IProductRepository
{
    public ProductRepository(YerbowoContext db) : base(db)
    { }

    public async Task<Product> GetAsync(string slug)
    {
        return await _entitiesNotRemoved.SingleOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<Product> GetWithCategoryAsync(Guid productId)
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
        var productIds = await _entitiesNotRemoved
            .AsNoTracking()
            .OrderBy(x => Guid.NewGuid())
            .Select(x => x.Id)
            .Take(quantity)
            .ToListAsync();

        var products = await _entitiesNotRemoved
            .Include(x => x.Subcategory)
            .ThenInclude(x => x.Category)
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToListAsync();

        return products
            .OrderBy(x => productIds.IndexOf(x.Id))
            .ToList();
    }

    public async Task<PagedList<Product>> BrowseAsync(
        int pageNumber,
        int pageSize,
        string category,
        string subcategory)
    {
        IQueryable<Product> products = _entitiesNotRemoved
            .Include(x => x.Subcategory)
            .ThenInclude(x => x.Category);

        if (!string.IsNullOrEmpty(subcategory))
        {
            products = products.Where(x => x.Subcategory.Slug == subcategory);
        }

        if (!string.IsNullOrEmpty(category))
        {
            products = products.Where(x => x.Subcategory.Category.Slug == category);
        }

        products = products
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ThenBy(x => x.Id);

        return await PagedList<Product>.CreateAsync(products, pageNumber, pageSize);
    }
}