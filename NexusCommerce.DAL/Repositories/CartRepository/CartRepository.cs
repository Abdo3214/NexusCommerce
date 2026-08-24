using Microsoft.EntityFrameworkCore;
using NexusCommerce.DAL.Data.Context;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.CartRepository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
