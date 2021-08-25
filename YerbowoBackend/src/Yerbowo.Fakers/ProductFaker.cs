using Bogus;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.Extensions;

namespace Yerbowo.Fakers
{
	public class ProductFaker : Faker<Product>
	{
		public ProductFaker()
		{
			Ignore(p => p.Id);
			RuleFor(p => p.Name, f => f.Commerce.ProductName());
			RuleFor(p => p.Name, f => f.Commerce.ProductName().ToSlug());
			RuleFor(p => p.Code, f => f.Commerce.Ean8());
			RuleFor(p => p.OldPrice, f => decimal.Parse(f.Commerce.Price()));
			RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()));
			RuleFor(p => p.Description, f => f.Commerce.ProductAdjective());
			RuleFor(p => p.Stock, f => f.Random.Int(0,100));
		}
	}
}
