using NexusCommerce.Common.Filtering;
using NexusCommerce.Common.Pagination;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.Repositories.GenericRepository;

namespace NexusCommerce.DAL.Repositories.ProductRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PagedResult<Product>> GetProductsPagedAsync(ProductFilterParameters filterParams);
        Task<Product?> GetProductWithCategoryAsync(int id);
    }
}
