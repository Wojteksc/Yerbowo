using Ardalis.GuardClauses;
using System.Collections.Generic;
using Yerbowo.Domain.Extensions;
using Yerbowo.Domain.SeedWork;
using Yerbowo.Domain.Subcategories;
using static Ardalis.GuardClauses.Guard;

namespace Yerbowo.Domain.Categories
{
    public class Category : BaseEntity
    {
        public string Name { get; protected set; }
        public string Slug { get; protected set; }
        public string Description { get; protected set; }
        public string Image { get; protected set; }

        public ICollection<Subcategory> Subcategories { get; protected set; }

        private Category() {}

        public Category(string name, string description, string image)
        {
            SetName(name);
            SetDescription(description);
            SetImage(image);
            SetSlug(name);
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
    }
}
