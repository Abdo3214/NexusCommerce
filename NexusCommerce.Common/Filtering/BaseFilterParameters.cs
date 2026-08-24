using NexusCommerce.Common.Pagination;

namespace NexusCommerce.Common.Filtering
{
    public class BaseFilterParameters : PaginationParameters
    {
        public string? SortBy { get; set; }
        public string SortOrder { get; set; } = "asc"; // asc or desc
    }
}
