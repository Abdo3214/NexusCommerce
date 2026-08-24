using AutoMapper;
using FluentValidation;
using NexusCommerce.BLL.DTOs.Product;
using NexusCommerce.BLL.Mappers.Errors;
using NexusCommerce.Common.Filtering;
using NexusCommerce.Common.GeneralResult;
using NexusCommerce.Common.Pagination;
using NexusCommerce.DAL.Data.Models;
using NexusCommerce.DAL.UnitOfWork;

namespace NexusCommerce.BLL.Managers.Product
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductCreateDto> _createValidator;
        private readonly IErrorMapper _errorMapper;

        public ProductManager(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<ProductCreateDto> createValidator,
            IErrorMapper errorMapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _errorMapper = errorMapper;
        }

        public async Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsPagedAsync(ProductFilterParameters filterParams)
        {
            var pagedProducts = await _unitOfWork.Products.GetProductsPagedAsync(filterParams);
            var dtos = _mapper.Map<IEnumerable<ProductReadDto>>(pagedProducts.Items);
            var pagedResult = new PagedResult<ProductReadDto>(
                dtos,
                pagedProducts.Metadata.TotalCount,
                pagedProducts.Metadata.CurrentPage,
                pagedProducts.Metadata.PageSize
            );
            return GeneralResult<PagedResult<ProductReadDto>>.SuccessResult(pagedResult);
        }

        public async Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductWithCategoryAsync(id);
            if (product == null)
            {
                return GeneralResult<ProductReadDto>.NotFound($"Product with ID {id} was not found.");
            }
            var dto = _mapper.Map<ProductReadDto>(product);
            return GeneralResult<ProductReadDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return GeneralResult<ProductReadDto>.FailResult(_errorMapper.MapValidationFailure(validationResult));
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(createDto.CategoryId);
            if (category == null)
            {
                return GeneralResult<ProductReadDto>.FailResult(Errors.CreateSingle("Category", "NotFound", $"Category with ID {createDto.CategoryId} does not exist."));
            }

            var product = _mapper.Map<DAL.Data.Models.Product>(createDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var addedProduct = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
            var dto = _mapper.Map<ProductReadDto>(addedProduct ?? product);
            return GeneralResult<ProductReadDto>.SuccessResult(dto, "Product created successfully.");
        }

        public async Task<GeneralResult<ProductReadDto>> UpdateProductAsync(ProductEditDto editDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(editDto.Id);
            if (product == null)
            {
                return GeneralResult<ProductReadDto>.NotFound($"Product with ID {editDto.Id} was not found.");
            }

            var category = await _unitOfWork.Categories.GetByIdAsync(editDto.CategoryId);
            if (category == null)
            {
                return GeneralResult<ProductReadDto>.FailResult(Errors.CreateSingle("Category", "NotFound", $"Category with ID {editDto.CategoryId} does not exist."));
            }

            _mapper.Map(editDto, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            var updatedProduct = await _unitOfWork.Products.GetProductWithCategoryAsync(product.Id);
            var dto = _mapper.Map<ProductReadDto>(updatedProduct ?? product);
            return GeneralResult<ProductReadDto>.SuccessResult(dto);
        }

        public async Task<GeneralResult> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return GeneralResult.NotFound($"Product with ID {id} was not found.");
            }

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult("Product deleted successfully.");
        }
    }
}
