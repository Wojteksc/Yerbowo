namespace Yerbowo.Application.Functions.Products.Query.GetProductDetails;

public record GetProductBySlugQuery(string Slug) : IQuery<ProductDetailsDto> { }