using System.Collections.Generic;
using Yerbowo.Domain.Products;
using Yerbowo.Fakers;
using Yerbowo.Fakers.Extensions;

namespace Yerbowo.Integration.Tests.Builders
{
	public class ProductGenerator
	{
		/// <summary>
		/// Generates products according to the given quantity with assigned concrete subcategory and category
		/// </summary>
		/// <param name="quantity">Number of products</param>
		/// <returns>products with assigned concrete subcategory and category</returns>
		public IEnumerable<Product> Generate(int quantity)
		{
			var subcategory = new SubcategoryFaker().UsePrivateConstructor().Generate();
			var category = new CategoryFaker().UsePrivateConstructor().Generate();

			for (int i = 0; i < quantity; i++)
			{
				var product = new ProductFaker().UsePrivateConstructor().Generate();
				product.SetSubcategory(subcategory);
				product.Subcategory.SetCategory(category);

				yield return product;
			}
		}

	}
}
