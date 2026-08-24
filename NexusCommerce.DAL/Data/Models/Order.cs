namespace NexusCommerce.DAL.Data.Models
{
    public class Order : IAuditable
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
