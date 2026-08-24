using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.CartRepository
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
    }
}
