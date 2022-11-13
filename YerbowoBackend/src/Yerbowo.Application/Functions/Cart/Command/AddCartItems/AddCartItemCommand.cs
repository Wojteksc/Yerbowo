﻿namespace Yerbowo.Application.Functions.Cart.Command.AddCartItems;

public class AddCartItemCommand : IRequest<CartDto>
{
	public int Id { get; }
	public int Quantity { get; }

	public AddCartItemCommand(int id, int quantity)
	{
		Id = id;
		Quantity = quantity;
	}
}