using Microsoft.AspNetCore.Mvc;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.CategoryDTOs;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost("[action]")]
        public IActionResult AddCategory([FromBody] AddCategoryDTO categoryDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.AddCategory(categoryDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.UpdateCategory(updateCategoryDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteCategory([FromQuery] Guid Id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.DeleteCategory(Id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCategoryByPage([FromQuery] int page = 1)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = await _categoryService.GetAllCategoryByPageAsync(headerLocale, page);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public IActionResult GetAllCategory()
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.GetAllCategory(headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public IActionResult GetCategoryDetailById([FromQuery] Guid Id)
        {

            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.GetCategoryDetailById(Id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
