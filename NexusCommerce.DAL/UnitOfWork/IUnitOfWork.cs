using NexusCommerce.DAL.Repositories.CartRepository;
using NexusCommerce.DAL.Repositories.CategoryRepository;
using NexusCommerce.DAL.Repositories.OrderRepository;
using NexusCommerce.DAL.Repositories.ProductRepository;

namespace NexusCommerce.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ICartRepository Carts { get; }
        IOrderRepository Orders { get; }
        Task<int> SaveChangesAsync();
    }
}
