using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class CategoryController : ControllerBase
    {
              private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _contextAccessor;
        public CategoryController(ICategoryService categoryService, IHttpContextAccessor contextAccessor)
        {
            _categoryService = categoryService;
            _contextAccessor = contextAccessor;
        }
        [HttpPost("[action]")]
        public IActionResult AddCategory([FromBody] AddCategoryDTO categoryDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = _categoryService.AddCategory(categoryDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UpdateCategory([FromBody] UpdateCategoryDTO updateCategoryDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _categoryService.UpdateCategory(updateCategoryDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteCategory([FromQuery] Guid Id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _categoryService.DeleteCategory(Id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCategoryByPage([FromQuery] int page = 1)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = await _categoryService.GetAllCategoryByPageAsync(headerLocale, page);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public IActionResult GetAllCategory()
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _categoryService.GetAllCategory(headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public IActionResult GetCategoryDetailById([FromQuery] Guid Id)
        {

            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _categoryService.GetCategoryDetailById(Id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
