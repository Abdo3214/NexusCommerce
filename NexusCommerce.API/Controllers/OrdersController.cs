using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NexusCommerce.BLL.DTOs.Order;
using NexusCommerce.BLL.Managers.Order;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        private bool IsAdmin => User.IsInRole("Admin");

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderCreateDto createDto)
        {
            var result = await _orderManager.PlaceOrderAsync(UserId, createDto);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _orderManager.GetUserOrdersAsync(UserId);
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _orderManager.GetOrderDetailsAsync(id, UserId, IsAdmin);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        private IActionResult MapError(GeneralResult result)
        {
            if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
    }
}
