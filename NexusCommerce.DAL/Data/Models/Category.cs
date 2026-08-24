namespace NexusCommerce.DAL.Data.Models
{
    public class Category : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
