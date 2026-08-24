namespace NexusCommerce.BLL.DTOs.Cart
{
    public class CartDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice => CartItems.Sum(item => item.ProductPrice * item.Quantity);
    }
}
