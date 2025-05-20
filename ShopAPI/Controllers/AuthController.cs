using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.AuthDTOs;


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
        [HttpGet("[action]")]
        [Authorize(Policy = "AllRole")]
        public IActionResult GetAllUserForSelect()
        {
            var result = _authService.GetAllUserForSelect();
             return StatusCode((int)result.StatusCode,result);;

        }
        [HttpGet("[action]")]
        [Authorize(Policy = "SuperAdminRole")]


        public async Task<IActionResult> GetAllUser([FromQuery] int page)
        {
            var result = await _authService.GetAllUserAsnyc(page);
             return StatusCode((int)result.StatusCode,result);;
        }
 
        [HttpPut("[action]")]
        [Authorize]
        public async Task<IActionResult> LogOut([FromQuery] Guid UserId)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.LogOutAsync(UserId.ToString(), headerLocale);

             return StatusCode((int)result.StatusCode,result);;
        }

        [HttpPut("[action]")]
        [Authorize(Policy = "AllRole")]
        public async Task<IActionResult> EditUserProfile([FromBody] UpdateUserDTO updateUserDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.EditUserProfileAsnyc(updateUserDTO, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpDelete("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid Id)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.DeleteUserAsnyc(Id, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpDelete("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleUserDTO removeRoleUserDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.RemoveRoleFromUserAsync(removeRoleUserDTO, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];

            var result = await _authService.RegisterAsync(registerDTO, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpPatch("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDTO assignRoleDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.AssignRoleToUserAsnyc(assignRoleDTO, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChecekdConfirmedEmailToken([FromBody] ConfirmedEmailDTO confirmedEmailDTO)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.ChecekdConfirmedEmailTokenAsnyc(confirmedEmailDTO.Email, confirmedEmailDTO.token, headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> SendEmailTokenForForgotPassword([FromQuery] string Email)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.SendEmailTokenForForgotPasswordAsync(Email,headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckTokenForForgotPassword([FromQuery] string Email, [FromQuery] string Token)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.CheckTokenForForgotPasswordAsync(Email, Token,headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePasswordForTokenForgotPassword([FromQuery] string Email, [FromQuery] string Token, [FromQuery] string NewPassword)
        {
            string headerLocale = _contextAccessor.HttpContext.Request?.Headers["Accept-Language"];
            var result = await _authService.ChangePasswordForTokenForgotPasswordAsync(Email, Token, NewPassword,headerLocale);
             return StatusCode((int)result.StatusCode,result);;
        }
    }
}
