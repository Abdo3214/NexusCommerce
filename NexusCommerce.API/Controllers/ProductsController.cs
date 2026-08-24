using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCommerce.BLL.DTOs.Product;
using NexusCommerce.BLL.Managers.Product;
using NexusCommerce.Common.Filtering;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] ProductFilterParameters filterParams)
        {
            var result = await _productManager.GetProductsPagedAsync(filterParams);
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productManager.GetProductByIdAsync(id);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto createDto)
        {
            var result = await _productManager.CreateProductAsync(createDto);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductEditDto editDto)
        {
            if (id != editDto.Id)
            {
                return BadRequest(new { Message = "ID in route path does not match ID in body." });
            }

            var result = await _productManager.UpdateProductAsync(editDto);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productManager.DeleteProductAsync(id);
            if (!result.Success)
            {
                return MapError(result);
            }
            return NoContent();
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
