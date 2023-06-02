﻿namespace Yerbowo.Application.Functions.Products.Command.ChangeProducts;

public class ChangeProductCommand : IRequest
{
    public int Id { get; set; }
    public int SubcategoryId { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; protected set; }
    public ProductState State { get; set; }
    public string Image { get; set; }
}