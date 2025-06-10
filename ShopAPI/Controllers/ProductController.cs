using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGetRequestLangService _getRequestLangService;

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        public ProductController(IProductService productService, IHttpContextAccessor contextAccessor, IGetRequestLangService getRequestLangService)
        {
            _productService = productService;
            _contextAccessor = contextAccessor;
            _getRequestLangService = getRequestLangService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductDTO addProductDTO)
        {
            
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"]?? DefaultLaunguage;
            var result = await _productService.AddProductAsync(addProductDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProduct([FromForm]UpdateProductDTO updateProductDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _productService.UpdateProductAsync(updateProductDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteProduct([FromQuery] Guid id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = _productService.DeleteProduct(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductById([FromQuery] Guid id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _productService.GetProductByIdAsync(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByIdForUpdate([FromQuery] Guid id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _productService.GetProductByIdForUpdateAsync(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProductByPage([FromQuery] int page, [FromQuery] string? search = null)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _productService.GetAllProductByPageOrSearchAsync(page, headerLocale, search);
            return StatusCode((int)result.StatusCode,result);

        }
        [HttpGet("[action]")]
        public IActionResult GetProductByFeatured()
        {
            string headerLocale = _getRequestLangService.GetRequestLanguage();
            var result = _productService.GetProductByFeatured(headerLocale);
            return StatusCode((int)result.StatusCode, result);


        }
    }
}
