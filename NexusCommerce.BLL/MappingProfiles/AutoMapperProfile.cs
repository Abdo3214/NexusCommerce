using AutoMapper;
using NexusCommerce.BLL.DTOs.Category;
using NexusCommerce.BLL.DTOs.Product;
using NexusCommerce.BLL.DTOs.Cart;
using NexusCommerce.BLL.DTOs.Order;
using NexusCommerce.DAL.Data.Models;

namespace NexusCommerce.BLL.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryEditDto, Category>();

            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductEditDto, Product>();

            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0m))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty));

            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty));
        }
    }
}
