using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.AuthDTOs;
using System.Security.Claims;


namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthController(IAuthServices authService, IHttpContextAccessor contextAccessor)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result=await _authService.LoginAsync(loginDTO, headerLocale);
            return StatusCode((int)result.StatusCode,result);

        }

    }
}
