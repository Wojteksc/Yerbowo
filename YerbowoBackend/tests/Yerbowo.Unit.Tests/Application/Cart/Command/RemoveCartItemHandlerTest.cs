using Yerbowo.Application.Functions.Cart.Command.RemoveCartItems;

namespace Yerbowo.Unit.Tests.Application.Cart.Command;

public class RemoveCartItemHandlerTest
{
    private readonly CartProductItemDto _cartProductItem;

    const int _productId = 1000;

    public RemoveCartItemHandlerTest()
    {
        _cartProductItem = new CartProductItemDto
        {
            Id = _productId,
            Code = "code",
            Name = "name",
            Description = "description",
            Price = 34m,
            OldPrice = 34m,
            Stock = 10,
            State = ProductState.None,
            Image = "image.png",
            Slug = "name".ToSlug()
        };
    }

    [Fact]
    public async Task Should_RemoveProductFromCart()
    {
        var request = new RemoveCartItemCommand(_productId);

        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        var expectedCartProducts = new List<CartItemDto>();

        var sessionMock = SessionMockHelper.SetupSession(Consts.CartSessionKey, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var result = await handler.Handle(request, CancellationToken.None);

        result.Items.Should().BeEquivalentTo(expectedCartProducts);
        sessionMock.Verify(x => x.Set(Consts.CartSessionKey, It.IsAny<byte[]>()), Times.Once);
    }

    [Fact]
    public async Task Should_ThrowException_When_ProductDoesNotExistInCart()
    {
        int productId = 999;
        var request = new RemoveCartItemCommand(productId);

        var cartProducts = new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = _cartProductItem,
                Quantity = 1,
            }
        };

        var sessionMock = SessionMockHelper.SetupSession(Consts.CartSessionKey, cartProducts);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(request, CancellationToken.None));

        exception.Message.Should().Be("Nie znaleziono produktu.");
        sessionMock.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()), Times.Never());
    }

    private RemoveCartItemHandler CreateHandler(HttpContextAccessor httpContextAccessor)
    {
        return new RemoveCartItemHandler(
            httpContextAccessor,
            AutoMapperConfig.Initialize());
    }
}
