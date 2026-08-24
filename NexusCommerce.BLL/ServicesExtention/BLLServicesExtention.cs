using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NexusCommerce.BLL.Managers.Auth;
using NexusCommerce.BLL.Managers.Cart;
using NexusCommerce.BLL.Managers.Category;
using NexusCommerce.BLL.Managers.Image;
using NexusCommerce.BLL.Managers.Order;
using NexusCommerce.BLL.Managers.Product;
using NexusCommerce.BLL.Mappers.Errors;

namespace NexusCommerce.BLL.ServicesExtention
{
    public static class BLLServicesExtention
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            services.AddScoped<IErrorMapper, ErrorMapper>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IImageManager, ImageManager>();

            var mapperConfig = new AutoMapper.MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles.AutoMapperProfile());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            services.AddValidatorsFromAssembly(typeof(BLLServicesExtention).Assembly);

            return services;
        }
    }
}
