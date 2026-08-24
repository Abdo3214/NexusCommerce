using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByIdWithItemsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
