﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services.WebUI;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using Shop.Persistence;

namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class HomeSliderItemController : ControllerBase
    {
        private string DefaultLaunguage
        {
            get
            {
                return ConfigurationPersistence.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly IHomeSliderService _homeSliderItemService;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeSliderItemController(IHomeSliderService homeSliderService, IHttpContextAccessor contextAccessor)
        {
            _homeSliderItemService = homeSliderService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddHomeSliderItem([FromForm] AddHomeSliderItemDTO addHomeSliderItemDTO)

        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _homeSliderItemService.AddHomeSliderItemAsync(addHomeSliderItemDTO, LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
  
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateHomeSliderItem([FromForm] UpdateHomeSliderItemDTO updateHomeSliderItemDTO)

        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _homeSliderItemService.UpdateHomeSliderItemAsync(updateHomeSliderItemDTO,  LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
 
        [HttpDelete("[action]")]
        public IActionResult DeleteHomeSliderItem([FromQuery] Guid Id)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;

            var result = _homeSliderItemService.DeleteHomeSliderItem(Id,LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]

        public async Task<IActionResult> GetAllHomeSliderItemByPageOrSearch([FromQuery] int page, [FromQuery] string? search=null)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _homeSliderItemService.GetAllHomeSliderByPageOrSearchAsync(LangCode, page);
            return StatusCode((int)result.StatusCode, result);

        }
       
        [HttpGet("[action]")]
        public async Task<IActionResult> GetHomeSliderItemForUpdate([FromQuery] Guid Id)
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = await _homeSliderItemService.GetHomeSliderItemForUpdateAsync(Id, LangCode);

            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("[action]")]

        public IActionResult GetHomeSliderItemForUI()
        {
            string LangCode = _contextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString() ?? DefaultLaunguage;
            var result = _homeSliderItemService.GetHomeSliderItemForUI(LangCode);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
