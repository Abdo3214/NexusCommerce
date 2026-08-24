using NexusCommerce.BLL.DTOs.Cart;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Cart
{
    public interface ICartManager
    {
        Task<GeneralResult<CartDto>> GetCartByUserIdAsync(string userId);
        Task<GeneralResult<CartDto>> AddToCartAsync(string userId, int productId, int quantity);
        Task<GeneralResult<CartDto>> UpdateCartItemQuantityAsync(string userId, int productId, int quantity);
        Task<GeneralResult<CartDto>> RemoveFromCartAsync(string userId, int productId);
    }
}
