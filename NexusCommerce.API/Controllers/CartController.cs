using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NexusCommerce.BLL.Managers.Cart;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;

        public CartController(ICartManager cartManager)
        {
            _cartManager = cartManager;
        }

        private string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var result = await _cartManager.GetCartByUserIdAsync(UserId);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
        {
            var result = await _cartManager.AddToCartAsync(UserId, request.ProductId, request.Quantity);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItemRequest request)
        {
            var result = await _cartManager.UpdateCartItemQuantityAsync(UserId, request.ProductId, request.Quantity);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var result = await _cartManager.RemoveFromCartAsync(UserId, productId);
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

    public class CartItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
