namespace NexusCommerce.Common.Pagination
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public PaginationMetadata Metadata { get; set; } = new();

        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            Metadata = new PaginationMetadata(totalCount, pageNumber, pageSize);
        }
    }
}
