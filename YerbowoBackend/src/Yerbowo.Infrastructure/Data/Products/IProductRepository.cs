namespace Yerbowo.Infrastructure.Data.Products;

public interface IProductRepository : IEntityRepository<Product>
{
    Task<Product> GetAsync(string slug);

    Task<Product> GetAsync(string slug, Func<IQueryable<Product>, IQueryable<Product>> func);

    Task<bool> ExistsAsync(string slug);

    Task<PagedList<Product>> BrowseAsync(int pageNumber, int pageSize, string category, string subcategory);

    Task<IEnumerable<Product>> BrowseRandomAsync(int quantity);
}