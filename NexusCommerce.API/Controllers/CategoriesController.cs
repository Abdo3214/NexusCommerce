using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusCommerce.BLL.DTOs.Category;
using NexusCommerce.BLL.Managers.Category;
using NexusCommerce.Common.GeneralResult;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;

        public CategoriesController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryManager.GetAllCategoriesAsync();
            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryManager.GetCategoryByIdAsync(id);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto createDto)
        {
            var result = await _categoryManager.CreateCategoryAsync(createDto);
            if (!result.Success)
            {
                return MapError(result);
            }
            return Ok(result.Data);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryEditDto editDto)
        {
            if (id != editDto.Id)
            {
                return BadRequest(new { Message = "ID in route path does not match ID in body." });
            }

            var result = await _categoryManager.UpdateCategoryAsync(editDto);
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
            var result = await _categoryManager.DeleteCategoryAsync(id);
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
