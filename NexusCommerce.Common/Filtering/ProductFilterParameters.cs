namespace NexusCommerce.Common.Filtering
{
    public class ProductFilterParameters : BaseFilterParameters
    {
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
    }
}
