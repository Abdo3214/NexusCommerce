using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Repositories.CartRepository;
using NexusCommerce.DAL.Repositories.CategoryRepository;
using NexusCommerce.DAL.Repositories.OrderRepository;
using NexusCommerce.DAL.Repositories.ProductRepository;

namespace NexusCommerce.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public ICartRepository Carts { get; }
        public IOrderRepository Orders { get; }

        public UnitOfWork(AppDbContext context, IProductRepository products, ICategoryRepository categories, ICartRepository carts, IOrderRepository orders)
        {
            _context = context;
            Products = products;
            Categories = categories;
            Carts = carts;
            Orders = orders;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
