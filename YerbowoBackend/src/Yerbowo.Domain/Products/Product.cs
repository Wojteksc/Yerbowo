using Ardalis.GuardClauses;
using Yerbowo.Domain.Extensions;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Domain.Subcategories;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Products
{
	public class Product : BaseEntity
	{
		public int SubcategoryId { get; protected set; }

		public string Code { get; protected set; }
		public string Name { get; protected set; }
		public string Description { get; protected set; }
		public decimal Price { get; protected set; }
		public decimal OldPrice { get; protected set; }
		public int Stock { get; protected set; }
		public ProductState State { get; protected set; }
		public string Image { get; protected set; }
		public string Slug { get; protected set; }

		public Subcategory Subcategory { get; protected set; }

		private Product() {}

		public Product(int subcategoryId, string code, string name,
			string description, decimal price, decimal oldPrice,
			int stock, ProductState state, string image)

		{
			SetSubcategoryId(subcategoryId);
			SetCode(code);
			SetName(name);
			SetDescription(description);
			SetPrice(price);
			SetOldPrice(oldPrice);
			SetStock(stock);
			SetState(state);
			SetImage(image);
			SetSlug(name);
		}

		private void SetSubcategoryId(int subcategoryId)
		{
			Against.NegativeOrZero(subcategoryId, nameof(subcategoryId));
			SubcategoryId = subcategoryId;
		}

		public void SetCode(string code)
		{
			Against.NullOrEmpty(code, nameof(code));
			Code = code;
		}

		private void SetName(string name)
		{
			Against.NullOrEmpty(name, nameof(name));
			Name = name;
		}

		private void SetDescription(string description)
		{
			Against.NullOrEmpty(description, nameof(description));
			Description = description;
		}

		private void SetPrice(decimal price)
		{
			Against.Negative(price, nameof(price));
			Price = price;
		}

		private void SetOldPrice(decimal oldPrice)
		{
			Against.Negative(oldPrice, nameof(oldPrice));
			OldPrice = oldPrice;
		}

		private void SetStock(int stock)
		{
			Against.Negative(stock, nameof(stock));
			Stock = stock;
		}

		public void SetState(ProductState state)
		{
			State = state;
		}

		private void SetImage(string image)
		{
			Against.NullOrEmpty(image, nameof(image));
			Image = image;
		}

		private void SetSlug(string slug)
		{
			Against.NullOrEmpty(slug, nameof(slug));
			Slug = slug.ToSlug();
		}

		public void SetSubcategory(Subcategory subcategory)
		{
			Against.Null(subcategory, nameof(subcategory));
			Subcategory = subcategory;
		}
	}
}
