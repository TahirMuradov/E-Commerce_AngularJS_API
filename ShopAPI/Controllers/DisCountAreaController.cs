using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using Shop.Persistence;
using System.Threading.Tasks;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class DisCountAreaController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IDiscountAreaService _countAreaService;
        private readonly IHttpContextAccessor _contextAccessor;

        public DisCountAreaController(IDiscountAreaService countAreaService, IHttpContextAccessor contextAccessor)
        {
            _countAreaService = countAreaService;
            _contextAccessor = contextAccessor;
        }
        [Authorize(Policy = "AllRole")]
        [HttpPost("[action]")]
        public IActionResult AddDiscountArea([FromBody] AddDisCountAreaDTO addDisCountAreaDTO)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _countAreaService.AddDiscountArea(addDisCountAreaDTO, LangCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Policy = "AllRole")]
        [HttpPut("[action]")]
        public IActionResult UpdateDiscountArea([FromBody] UpdateDisCountAreaDTO updateDisCountAreaDTO)
        {

            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _countAreaService.UpdateDisCountArea(updateDisCountAreaDTO, LangCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Policy = "AllRole")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDisCountAreaForUpdate([FromQuery] Guid Id)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _countAreaService.GetDisCountAreaForUpdateAsync(Id,LangCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [HttpGet("[action]")]
        public IActionResult GetAllDisCountArea([FromHeader] string LangCode)
        {
            var result = _countAreaService.GetAllDisCountArea(LangCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [Authorize(Policy = "AllRole")]
        [HttpDelete("[action]")]
        public IActionResult DeleteDisCountArea([FromQuery] Guid Id)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;

            var result = _countAreaService.Delete(Id,LangCode);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    
        [HttpGet("[action]")]
        public async Task<IActionResult> GetDisCountAreaForTable([FromQuery] int page)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _countAreaService.GetAllDisCountForTableAsync(LangCode, page);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
