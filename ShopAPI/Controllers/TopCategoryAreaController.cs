using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class TopCategoryAreaController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly ITopCategoryAreaService _topCategoryAreaService;
        private readonly IHttpContextAccessor _contextAccessor;

        public TopCategoryAreaController(ITopCategoryAreaService topCategoryAreaService, IHttpContextAccessor contextAccessor)
        {
            _topCategoryAreaService = topCategoryAreaService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddTopCategoryArea([FromForm] AddTopCategoryAreaDTO addTopCategoryAreaDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _topCategoryAreaService.AddTopCategoryAreaAsync(addTopCategoryAreaDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);

        }
     
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateTopCategoryArea([FromForm] UpdateTopCategoryAreaDTO updateTopCategoryAreaDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _topCategoryAreaService.UpdateTopCategoryAreaAsync(updateTopCategoryAreaDTO, headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
   
        [HttpDelete("[action]")]
        public IActionResult RemoveTopCategoryArea([FromQuery] Guid Id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = _topCategoryAreaService.RemoveTopCategoryArea(Id,headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopCategoryAreaByPageOrSearch([FromQuery] int page, [FromQuery] string? search=null)
        {
            string LangCode = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _topCategoryAreaService.GetTopCategoryAreaByPageOrSearchAsync(LangCode, page);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetTopCategoryAreaForUI()
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = _topCategoryAreaService.GetTopCategoryAreaForUI(headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
       
        [HttpGet("[action]")]
        public async Task<IActionResult> GetTopCategoryAreaForUpdate([FromQuery] Guid Id)
        {

            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"] ?? DefaultLaunguage;
            var result = await _topCategoryAreaService.GetTopcategoryAreaForUpdateAsync(Id,headerLocale);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
