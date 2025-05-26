using Microsoft.AspNetCore.Http;
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
      public IActionResult AddCategory([FromBody]AddCategoryDTO categoryDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _categoryService.AddCategory(categoryDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
