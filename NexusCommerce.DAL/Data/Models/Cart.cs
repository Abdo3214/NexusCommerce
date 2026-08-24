namespace NexusCommerce.DAL.Data.Models
{
    public class Cart : IAuditable
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
