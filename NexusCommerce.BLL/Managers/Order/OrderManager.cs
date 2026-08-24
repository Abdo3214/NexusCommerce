using AutoMapper;
using NexusCommerce.BLL.DTOs.Order;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.BLL.Managers.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId, OrderCreateDto createDto)
        {
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                return GeneralResult<OrderReadDto>.FailResult(Errors.CreateSingle("Cart", "Empty", "Your shopping cart is empty."));
            }

            decimal totalPrice = 0;
            var orderItems = new List<OrderItem>();

            foreach (var cartItem in cart.CartItems)
            {
                var product = cartItem.Product;
                if (product == null)
                {
                    return GeneralResult<OrderReadDto>.FailResult(Errors.CreateSingle("Product", "NotFound", "A product in your cart could not be found."));
                }

                if (product.Stock < cartItem.Quantity)
                {
                    return GeneralResult<OrderReadDto>.FailResult(Errors.CreateSingle("Product", "InsufficientStock", $"Product '{product.Name}' has insufficient stock. Available: {product.Stock}."));
                }

                product.Stock -= cartItem.Quantity;
                _unitOfWork.Products.Update(product);

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                };
                orderItems.Add(orderItem);

                totalPrice += product.Price * cartItem.Quantity;
            }

            var order = new DAL.Data.Models.Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice,
                Status = "Processing",
                OrderItems = orderItems,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Orders.AddAsync(order);

            cart.CartItems.Clear();
            _unitOfWork.Carts.Update(cart);

            await _unitOfWork.SaveChangesAsync();

            var createdOrder = await _unitOfWork.Orders.GetOrderByIdWithItemsAsync(order.Id);
            var dto = _mapper.Map<OrderReadDto>(createdOrder ?? order);
            return GeneralResult<OrderReadDto>.SuccessResult(dto, "Order placed successfully.");
        }

        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetUserOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<OrderReadDto>>(orders);
            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessResult(dtos);
        }

        public async Task<GeneralResult<OrderReadDto>> GetOrderDetailsAsync(int orderId, string userId, bool isAdmin)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdWithItemsAsync(orderId);
            if (order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound($"Order with ID {orderId} was not found.");
            }

            if (order.UserId != userId && !isAdmin)
            {
                return GeneralResult<OrderReadDto>.FailResult(Errors.CreateSingle("Order", "Forbidden", "You do not have access to view this order."));
            }

            var dto = _mapper.Map<OrderReadDto>(order);
            return GeneralResult<OrderReadDto>.SuccessResult(dto);
        }
    }
}
