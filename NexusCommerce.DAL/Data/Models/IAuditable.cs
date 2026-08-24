namespace NexusCommerce.DAL.Data.Models
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
