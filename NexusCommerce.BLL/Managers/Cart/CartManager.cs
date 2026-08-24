using AutoMapper;
using NexusCommerce.BLL.DTOs.Cart;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.BLL.Managers.Cart
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task<DAL.Data.Models.Cart> GetOrCreateCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new DAL.Data.Models.Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            }
            return cart!;
        }

        public async Task<GeneralResult<CartDto>> GetCartByUserIdAsync(string userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var dto = _mapper.Map<CartDto>(cart);
            return GeneralResult<CartDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult<CartDto>> AddToCartAsync(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return GeneralResult<CartDto>.FailResult(Errors.CreateSingle("Cart", "InvalidQuantity", "Quantity must be greater than zero."));
            }

            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
            {
                return GeneralResult<CartDto>.NotFound($"Product with ID {productId} does not exist.");
            }

            if (product.Stock < quantity)
            {
                return GeneralResult<CartDto>.FailResult(Errors.CreateSingle("Product", "InsufficientStock", $"Only {product.Stock} items are in stock."));
            }

            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                var newQty = cartItem.Quantity + quantity;
                if (product.Stock < newQty)
                {
                    return GeneralResult<CartDto>.FailResult(Errors.CreateSingle("Product", "InsufficientStock", $"Cannot add more. Total cart quantity {newQty} exceeds available stock of {product.Stock}."));
                }
                cartItem.Quantity = newQty;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _unitOfWork.SaveChangesAsync();

            cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            var dto = _mapper.Map<CartDto>(cart);
            return GeneralResult<CartDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult<CartDto>> UpdateCartItemQuantityAsync(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
            {
                return await RemoveFromCartAsync(userId, productId);
            }

            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
            {
                return GeneralResult<CartDto>.NotFound($"Product with ID {productId} does not exist.");
            }

            if (product.Stock < quantity)
            {
                return GeneralResult<CartDto>.FailResult(Errors.CreateSingle("Product", "InsufficientStock", $"Only {product.Stock} items are in stock."));
            }

            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return GeneralResult<CartDto>.NotFound("Product is not in the cart.");
            }

            cartItem.Quantity = quantity;
            await _unitOfWork.SaveChangesAsync();

            cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            var dto = _mapper.Map<CartDto>(cart);
            return GeneralResult<CartDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult<CartDto>> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                return GeneralResult<CartDto>.NotFound("Product is not in the cart.");
            }

            cart.CartItems.Remove(cartItem);
            await _unitOfWork.SaveChangesAsync();

            cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            var dto = _mapper.Map<CartDto>(cart);
            return GeneralResult<CartDto>.SuccessResult(dto);
        }
    }
}
