using System;
using Yerbowo.Domain.Products;

namespace Yerbowo.Application.Products
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategorySlug { get; set; }
        public string SubcategorySlug { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public ProductState State { get; set; }
        public string Image { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
