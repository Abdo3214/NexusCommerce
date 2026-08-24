using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Repositories.CartRepository;
using NexusCommerce.DAL.Repositories.CategoryRepository;
using NexusCommerce.DAL.Repositories.OrderRepository;
using NexusCommerce.DAL.Repositories.ProductRepository;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.DAL.ServicesExtention
{
    public static class DALServicesExtention
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
