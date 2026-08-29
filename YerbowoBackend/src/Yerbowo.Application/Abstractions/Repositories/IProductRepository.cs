namespace Yerbowo.Application.Abstractions.Repositories;

public interface IProductRepository : IDbEntityRepository<Product>
{
    Task<Product> GetAsync(string slug);
    
    Task<Product> GetWithCategoryAsync(Guid productId);

    Task<Product> GetWithCategoryAsync(string slug);

    Task<bool> ExistsAsync(string slug);

    Task<PagedList<Product>> BrowseAsync(int pageNumber, int pageSize, string category, string subcategory);

    Task<IEnumerable<Product>> BrowseRandomAsync(int quantity);
}