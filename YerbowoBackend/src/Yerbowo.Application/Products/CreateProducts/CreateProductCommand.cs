using MediatR;
using Yerbowo.Domain.Products;

namespace Yerbowo.Application.Products.CreateProducts
{
    public class CreateProductCommand : IRequest<CreateProductCommand>
    {
        public int Id { get; set; }
        public int SubcategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int Stock { get; set; }
        public ProductState State { get; set; }
        public string Image { get; set; }

    }
}
