using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class HomeController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IHomeService _homeService;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(IHomeService homeService, IHttpContextAccessor contextAccessor)
        {
            _homeService = homeService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllData()
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _homeService.GetHomeAllData(LangCode);
            return StatusCode((int)result.StatusCode, result);


        }
    }
}
