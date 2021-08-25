using Ardalis.GuardClauses;
using System.Collections.Generic;
using Yerbowo.Domain.Categories;
using Yerbowo.Domain.Extensions;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.SeedWork;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Subcategories
{
    public class Subcategory : BaseEntity
    {
        public int CategoryId { get; protected set; }

        public string Name { get; protected set; }
        public string Slug { get; protected set; }
        public string Description { get; protected set; }
        public string Image { get; protected set; }

        public Category Category { get; protected set; }
        public ICollection<Product> Products { get; protected set; }

        private Subcategory() {}

        public Subcategory(int categoryId, string name, string description, string image)
        {
            SetCategoryId(categoryId);
            SetName(name);
            SetDescription(description);
            SetImage(image);
            SetSlug(name);
        }

        private void SetCategoryId(int categoryId)
        {
            Against.NegativeOrZero(categoryId, nameof(categoryId));
            CategoryId = categoryId;
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

        public void SetCategory(Category category)
        {
            Against.Null(category, nameof(category));
            Category = category;
        }

    }
}
