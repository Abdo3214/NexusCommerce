namespace NexusCommerce.BLL.DTOs.Order
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
