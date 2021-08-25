namespace Yerbowo.Application.Orders.GetOrderDetails
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductCategorySlug { get; set; }
        public string ProductSubcategorySlug { get; set; }
        public string ProductSlug { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
}
