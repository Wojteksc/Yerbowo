using AutoMapper;
using EnumsNET;
using System.Collections.Generic;
using System.Linq;
using Yerbowo.Application.Addresses.ChangeAddresses;
using Yerbowo.Application.Addresses.CreateAddresses;
using Yerbowo.Application.Addresses.GetAddressDetails;
using Yerbowo.Application.Addresses.GetAddresses;
using Yerbowo.Application.Auth.Register;
using Yerbowo.Application.Auth.SocialLogin;
using Yerbowo.Application.Cart;
using Yerbowo.Application.Orders.GetOrderDetails;
using Yerbowo.Application.Orders.GetOrders;
using Yerbowo.Application.Products;
using Yerbowo.Application.Products.ChangeProducts;
using Yerbowo.Application.Products.CreateProducts;
using Yerbowo.Application.Products.GetPagedProducts;
using Yerbowo.Application.Products.GetProductDetails;
using Yerbowo.Application.Products.GetRandomProducts;
using Yerbowo.Application.Users.ChangeUsers;
using Yerbowo.Domain.Addresses;
using Yerbowo.Domain.Orders;
using Yerbowo.Domain.Products;
using Yerbowo.Domain.Users;
using Yerbowo.Infrastructure.Helpers;

namespace Yerbowo.Application.Settings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDetailsDto>();
                cfg.CreateMap<List<List<ProductCardDto>>, RandomProductsDto>();
                cfg.CreateMap<Product, ProductCardDto>()
                .ForMember(d => d.CategorySlug, s => s.MapFrom(x => x.Subcategory.Category.Slug))
                .ForMember(d => d.SubcategorySlug, s => s.MapFrom(x => x.Subcategory.Slug));
                cfg.CreateMap<CreateProductCommand, Product>();
                cfg.CreateMap<ChangeProductCommand, Product>();
                cfg.CreateMap<Product, CreateProductCommand>();
                cfg.CreateMap<PagedList<Product>, PagedProductCardDto>()
                .ForMember(d => d.Products, s => s.MapFrom(x => x));
                cfg.CreateMap<Product, CartProductItemDto>()
                .ForMember(d => d.CategorySlug, s => s.MapFrom(x => x.Subcategory.Category.Slug))
                .ForMember(d => d.SubcategorySlug, s => s.MapFrom(x => x.Subcategory.Slug));
                cfg.CreateMap<Product, ProductDetailsDto>()
                .ForMember(d => d.Category, s => s.MapFrom(x => x.Subcategory.Category.Name))
                .ForMember(d => d.Subcategory, s => s.MapFrom(x => x.Subcategory.Name));

                cfg.CreateMap<Order, OrderDto>()
                .ForMember(d => d.Total, s => s.MapFrom(x => x.TotalCost))
                .ForMember(d => d.Date, s => s.MapFrom(x => x.CreatedAt.ToString()))
                .ForMember(d => d.Status, s => s.MapFrom(x => (x.OrderStatus).AsString(EnumFormat.Description)))
                .ForMember(d => d.ProductImages, s => s.MapFrom(x => x.OrderItems));
                cfg.CreateMap<OrderItem, OrderProductImageDto>()
                .ForMember(d => d.Quantity, s => s.MapFrom(x => x.Quantity))
                .ForMember(d => d.Name, s => s.MapFrom(x => x.Product.Image));

                cfg.CreateMap<Order, OrderDetailsDto>()
                .ForMember(d => d.Address, s => s.MapFrom(x => x.Address))
                .ForMember(d => d.OrderItems, s => s.MapFrom(x => x.OrderItems))
                .ForMember(d => d.TotalCost, s => s.MapFrom(x => x.TotalCost));
                cfg.CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.Sum, s => s.MapFrom(x => x.Quantity * x.Price))
                .ForMember(d => d.ProductImage, s => s.MapFrom(x => x.Product.Image))
                .ForMember(d => d.ProductName, s => s.MapFrom(x => x.Product.Name))
                .ForMember(d => d.ProductCategorySlug, s => s.MapFrom(x => x.Product.Subcategory.Category.Slug))
                .ForMember(d => d.ProductSubcategorySlug, s => s.MapFrom(x => x.Product.Subcategory.Slug));

                cfg.CreateMap<Address, AddressDto>();
                cfg.CreateMap<CreateAddressCommand, Address>();
                cfg.CreateMap<ChangeAddressCommand, Address>();
                cfg.CreateMap<Address, CreateAddressCommand>();
                cfg.CreateMap<Address, AddressDetailsDto>();

                cfg.CreateMap<ChangeUserCommand, User>();
                cfg.CreateMap<RegisterCommand, User>();
                cfg.CreateMap<SocialLoginCommand, User>();

                cfg.CreateMap<List<CartItemDto>, CartDto>()
                .ForMember(d => d.Items, s => s.MapFrom(x => x))
                .ForMember(d => d.Sum, s => s.MapFrom(x => x.Sum(a => a.Product.Price * a.Quantity)))
                .ForMember(d => d.TotalItems, s => s.MapFrom(x => x.Sum(a => a.Quantity)));

            }).CreateMapper();
        }
    }
}
