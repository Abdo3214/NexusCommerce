using AutoMapper;
using FluentValidation;
using NexusCommerce.BLL.DTOs.Category;
using NexusCommerce.BLL.Mappers.Errors;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.BLL.Managers.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryCreateDto> _createValidator;
        private readonly IErrorMapper _errorMapper;

        public CategoryManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CategoryCreateDto> createValidator,
            IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessResult(dtos);
        }

        public async Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound($"Category with ID {id} was not found.");
            }
            var dto = _mapper.Map<CategoryReadDto>(category);
            return GeneralResult<CategoryReadDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult<CategoryReadDto>> CreateCategoryAsync(CategoryCreateDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return GeneralResult<CategoryReadDto>.FailResult(_errorMapper.MapValidationFailure(validationResult));
            }

            var category = _mapper.Map<DAL.Data.Models.Category>(createDto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CategoryReadDto>(category);
            return GeneralResult<CategoryReadDto>.SuccessResult(dto, "Category created successfully.");
        }

        public async Task<GeneralResult<CategoryReadDto>> UpdateCategoryAsync(CategoryEditDto editDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(editDto.Id);
            if (category == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound($"Category with ID {editDto.Id} was not found.");
            }

            _mapper.Map(editDto, category);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<CategoryReadDto>(category);
            return GeneralResult<CategoryReadDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return GeneralResult.NotFound($"Category with ID {id} was not found.");
            }

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult("Category deleted successfully.");
        }
    }
}
