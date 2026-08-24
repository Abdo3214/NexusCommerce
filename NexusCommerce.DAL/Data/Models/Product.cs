namespace NexusCommerce.DAL.Data.Models
{
    public class Product : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
