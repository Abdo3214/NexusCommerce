using NexusCommerce.BLL.DTOs.Category;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.BLL.Managers.Category
{
    public interface ICategoryManager
    {
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync();
        Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id);
        Task<GeneralResult<CategoryReadDto>> CreateCategoryAsync(CategoryCreateDto createDto);
        Task<GeneralResult<CategoryReadDto>> UpdateCategoryAsync(CategoryEditDto editDto);
        Task<GeneralResult> DeleteCategoryAsync(int id);
    }
}
