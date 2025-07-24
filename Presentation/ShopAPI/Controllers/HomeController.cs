using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Services.WebUI;


namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class HomeController : ControllerBase
    {
     
        private readonly IHomeService _homeService;
        private readonly IGetRequestLangService _getRequestLangService;

        public HomeController(IHomeService homeService, IGetRequestLangService getRequestLangService)
        {
            _homeService = homeService;
            _getRequestLangService = getRequestLangService;
        }

        [HttpGet("[action]")]
        public IActionResult GetAllData()
        {
            
            var result = _homeService.GetHomeAllData(_getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result);


        }
    }
}
