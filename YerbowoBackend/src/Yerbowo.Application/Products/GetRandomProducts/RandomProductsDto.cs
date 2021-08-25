using System.Collections.Generic;

namespace Yerbowo.Application.Products.GetRandomProducts
{
    public class RandomProductsDto
    {
        public IEnumerable<ProductCardDto> Bestsellers { get; protected set; }

        public IEnumerable<ProductCardDto> News { get; protected set; }

        public IEnumerable<ProductCardDto> Promotions { get; protected set; }
    }
}
