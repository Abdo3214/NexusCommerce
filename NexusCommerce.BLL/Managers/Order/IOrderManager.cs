using NexusCommerce.BLL.DTOs.Order;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Order
{
    public interface IOrderManager
    {
        Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId, OrderCreateDto createDto);
        Task<GeneralResult<IEnumerable<OrderReadDto>>> GetUserOrdersAsync(string userId);
        Task<GeneralResult<OrderReadDto>> GetOrderDetailsAsync(int orderId, string userId, bool isAdmin);
    }
}
