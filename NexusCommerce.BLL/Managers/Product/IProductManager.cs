using NexusCommerce.BLL.DTOs.Product;
using NexusCommerce.Common.Filtering;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.Common.Pagination;

namespace NexusCommerce.BLL.Managers.Product
{
    public interface IProductManager
    {
        Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsPagedAsync(ProductFilterParameters filterParams);
        Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id);
        Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto createDto);
        Task<GeneralResult<ProductReadDto>> UpdateProductAsync(ProductEditDto editDto);
        Task<GeneralResult> DeleteProductAsync(int id);
    }
}
