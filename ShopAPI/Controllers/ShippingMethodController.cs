using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.ShippingMethodDTOs;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class ShippingMethodController : ControllerBase
    {
        private readonly IShippingMethodService _shippingMethodService;

        public ShippingMethodController(IShippingMethodService shippingMethodService)
        {
            _shippingMethodService = shippingMethodService;
        }
        [HttpPost("[action]")]
        public IActionResult AddShippingMethod([FromBody]AddShippingMethodDTO addShippingMethodDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _shippingMethodService.AddShippingMethod(addShippingMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UdpateShippingMethod([FromBody]UpdateShippingMethodDTO updateShippingMethodDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _shippingMethodService.UdpateShippingMethod(updateShippingMethodDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteShippingMethod([FromQuery]Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _shippingMethodService.DeleteShippingMethod(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetShippingMethodById([FromQuery] Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _shippingMethodService.GetShippingMethodById(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllShippingMethods()
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _shippingMethodService.GetAllShippingMethods(headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllShippingMethodsByPage([FromQuery]int page = 1)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
       var result= await _shippingMethodService.GetAllShippingMethodsByPageAsync(page, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
