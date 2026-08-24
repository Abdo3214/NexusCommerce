using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCommerce.BLL.DTOs.Image;
using NexusCommerce.BLL.Managers.Image;
using NexusCommerce.BLL.Managers.Product;
using NexusCommerce.BLL.Managers.Category;
using NexusCommerce.BLL.DTOs.Product;
using NexusCommerce.BLL.DTOs.Category;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;

        public ImagesController(
            IImageManager imageManager,
            IProductManager productManager,
            ICategoryManager categoryManager)
        {
            _imageManager = imageManager;
            _productManager = productManager;
            _categoryManager = categoryManager;
        }

        [HttpPost("api/image/upload")]
        public async Task<IActionResult> UploadGeneral([FromForm] ImageUploadDto uploadDto)
        {
            var result = await _imageManager.UploadImageAsync(uploadDto);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPost("api/products/{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadProductImage(int id, [FromForm] ImageUploadDto uploadDto)
        {
            var productResult = await _productManager.GetProductByIdAsync(id);
            if (!productResult.Success)
            {
                return MapError(productResult);
            }

            var uploadResult = await _imageManager.UploadImageAsync(uploadDto);
            if (!uploadResult.Success)
            {
                return MapError(uploadResult);
            }

            var product = productResult.Data!;
            var editDto = new ProductEditDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = uploadResult.Data!.Url,
                CategoryId = product.CategoryId
            };

            var updateResult = await _productManager.UpdateProductAsync(editDto);
            if (!updateResult.Success)
            {
                return MapError(updateResult);
            }

            return Ok(uploadResult.Data);
        }

        [HttpPost("api/categories/{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UploadCategoryImage(int id, [FromForm] ImageUploadDto uploadDto)
        {
            var categoryResult = await _categoryManager.GetCategoryByIdAsync(id);
            if (!categoryResult.Success)
            {
                return MapError(categoryResult);
            }

            var uploadResult = await _imageManager.UploadImageAsync(uploadDto);
            if (!uploadResult.Success)
            {
                return MapError(uploadResult);
            }

            var category = categoryResult.Data!;
            var editDto = new CategoryEditDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = uploadResult.Data!.Url
            };

            var updateResult = await _categoryManager.UpdateCategoryAsync(editDto);
            if (!updateResult.Success)
            {
                return MapError(updateResult);
            }

            return Ok(uploadResult.Data);
        }

        private IActionResult MapError(GeneralResult result)
        {
            if (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }
    }
}
