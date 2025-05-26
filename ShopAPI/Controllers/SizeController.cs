using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.SizeDTOs;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        [HttpPost("[action]")]
        public IActionResult AddSize([FromBody] AddSizeDTO addSizeDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _sizeService.AddSize(addSizeDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpPut("[action]")]
        public IActionResult UpdateSize([FromBody]UpdateSizeDTO updateSizeDTO)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _sizeService.UpdateSize(updateSizeDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("[action]")]
        public IActionResult DeleteSize([FromQuery]Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _sizeService.DeleteSize(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        public IActionResult GetSizeById([FromQuery]Guid id)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _sizeService.GetSizeById(id, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllSizes()
        {

            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = _sizeService.GetAllSizes(headerLocale);
            return StatusCode((int)result.StatusCode, result);


        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllSizesByPage([FromQuery]int page)
        {
            string headerLocale = HttpContext.Request.Headers["Accept-Language"];
            var result = await _sizeService.GetAllSizesByPageAsync(page, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }

    }
}
