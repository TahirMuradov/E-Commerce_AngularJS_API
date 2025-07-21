using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class NewArriwalAreaController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return ConfigurationPersistence.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly INewArriwalAreaService _newArriwalService;
        private readonly IHttpContextAccessor _contextAccessor;

        public NewArriwalAreaController(INewArriwalAreaService newArriwalService, IHttpContextAccessor contextAccessor)
        {
            _newArriwalService = newArriwalService;
            _contextAccessor = contextAccessor;
        }
        [HttpGet("[action]")]
        public IActionResult GetNewArriwalProduct()
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _newArriwalService.GetNewArriwalProducts(LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]
        public IActionResult GetNewArriwalCategories()
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _newArriwalService.GetNewArriwalCategories(LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
