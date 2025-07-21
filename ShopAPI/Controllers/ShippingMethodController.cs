using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.ShippingMethodDTOs;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class ShippingMethodController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return ConfigurationPersistence.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IHttpContextAccessor _contextAccessor;
        public ShippingMethodController(IShippingMethodService shippingMethodService, IHttpContextAccessor contextAccessor)
        {
            _shippingMethodService = shippingMethodService;
            _contextAccessor = contextAccessor;
        }
        [HttpPost("[action]")]
        public IActionResult AddShippingMethod([FromBody] AddShippingMethodDTO addShippingMethodDTO)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _shippingMethodService.AddShippingMethod(addShippingMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UdpateShippingMethod([FromBody] UpdateShippingMethodDTO updateShippingMethodDTO)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _shippingMethodService.UdpateShippingMethod(updateShippingMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteShippingMethod([FromQuery] Guid id)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _shippingMethodService.DeleteShippingMethod(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetShippingMethodById([FromQuery] Guid id)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _shippingMethodService.GetShippingMethodById(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllShippingMethods()
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = _shippingMethodService.GetAllShippingMethods(headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllShippingMethodsByPage([FromQuery] int page = 1, [FromQuery] string? search=null)
        {
            string? headerLocale = _contextAccessor?.HttpContext?.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;

            var result = await _shippingMethodService.GetAllShippingMethodsByPageOrSearchAsync(page, headerLocale,search);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
