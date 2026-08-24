namespace NexusCommerce.BLL.DTOs.Category
{
    public class CategoryReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
