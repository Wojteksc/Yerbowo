﻿namespace Yerbowo.Unit.Tests.Application.Cart.Query;

public class GetCartItemsHandlerTest
{
    [Fact]
    public async Task Should_GetCartItemsCorrectly()
    {
        var sessionData = GetSessionData();
        var expectedData = GetExpectedData();

        var sessionMock = SessionMockHelper.SetupSession(Consts.CartSessionKey, sessionData);
        var httpContextAccessor = HttpContextAccessorFactory.Create(sessionMock);
        var handler = CreateHandler(httpContextAccessor);

        var result = await handler.Handle(new GetCartItemsQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(expectedData);
    }

    private IList<CartItemDto> GetSessionData()
    {
        return new List<CartItemDto>
        {
            new CartItemDto
            {
                Product = new CartProductItemDto
                {
                    Id = 1,
                    Code = "code1",
                    Name = "name1",
                    Description = "description1",
                    Price = 34m,
                    OldPrice = 34m,
                    Stock = 10,
                    State = ProductState.None,
                    Image = "image1.png",
                    Slug = "name1".ToSlug()
                },
                Quantity = 5
            },
            new CartItemDto
            {
                Product = new CartProductItemDto
                {
                    Id = 2,
                    Code = "code2",
                    Name = "name2",
                    Description = "description2",
                    Price = 25.99m,
                    OldPrice = 22m,
                    Stock = 20,
                    State = ProductState.None,
                    Image = "image2.png",
                    Slug = "name2".ToSlug()
                },
                Quantity = 4
            },
            new CartItemDto
            {
                Product = new CartProductItemDto
                {
                    Id = 3,
                    Code = "code3",
                    Name = "name3",
                    Description = "description3",
                    Price = 19.99m,
                    OldPrice = 18.99m,
                    Stock = 9,
                    State = ProductState.None,
                    Image = "image3.png",
                    Slug = "name3".ToSlug()
                },
                Quantity = 1
            }
        };
    }

    private CartDto GetExpectedData()
    {
        return new CartDto
        {
            TotalItems = 5 + 4 + 1,
            Sum = 170 + 103.96m + 19.99m,
            Items = new List<CartItemDto>
            {
                new CartItemDto
                {
                    Product = new CartProductItemDto
                    {
                        Id = 1,
                        Code = "code1",
                        Name = "name1",
                        Description = "description1",
                        Price = 34m,
                        OldPrice = 34m,
                        Stock = 10,
                        State = ProductState.None,
                        Image = "image1.png",
                        Slug = "name1".ToSlug()
                    },
                    Quantity = 5
                },
                new CartItemDto
                {
                    Product = new CartProductItemDto
                    {
                        Id = 2,
                        Code = "code2",
                        Name = "name2",
                        Description = "description2",
                        Price = 25.99m,
                        OldPrice = 22m,
                        Stock = 20,
                        State = ProductState.None,
                        Image = "image2.png",
                        Slug = "name2".ToSlug()
                    },
                    Quantity = 4
                },
                new CartItemDto
                {
                    Product = new CartProductItemDto
                    {
                        Id = 3,
                        Code = "code3",
                        Name = "name3",
                        Description = "description3",
                        Price = 19.99m,
                        OldPrice = 18.99m,
                        Stock = 9,
                        State = ProductState.None,
                        Image = "image3.png",
                        Slug = "name3".ToSlug()
                    },
                    Quantity = 1
                }
            }
        };
    }

    private GetCartItemsHandler CreateHandler(HttpContextAccessor httpContextAccessor)
    {
        return new GetCartItemsHandler(
            httpContextAccessor,
            AutoMapperConfig.Initialize());
    }
}