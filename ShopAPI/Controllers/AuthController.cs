using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shop.Application.Abstraction.Services;
using Shop.Application.DTOs.AuthDTOs;
using Shop.Persistence;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
     private readonly IGetRequestLangService _getRequestLangService;
        public AuthController(IAuthService authService, IGetRequestLangService getRequestLangService, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _getRequestLangService = getRequestLangService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            
            var result = await _authService.LoginAsync(loginDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result);

        }
        [HttpGet("[action]")]
        [Authorize(Policy = "AllRole")]
        public IActionResult GetAllUserForSelect()
        {
            var result = _authService.GetAllUserForSelect();
            return StatusCode((int)result.StatusCode, result); ;

        }
        [HttpGet("[action]")]
        [Authorize(Policy = "SuperAdminRole")]


        public async Task<IActionResult> GetAllUserByPageOrSearch([FromQuery] int page, [FromQuery] string? search=null)
        {
            var result = await _authService.GetAllUserByPageOrSearchAsync(page,search);
            return StatusCode((int)result.StatusCode, result); ;
        }

        [HttpPut("[action]")]

        public async Task<IActionResult> LogOut()
        {
  
            string? currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _authService.LogOutAsync(currentUserId,_getRequestLangService.GetRequestLanguage());

            return StatusCode((int)result.StatusCode, result); ;
        }

        [HttpPut("[action]")]
        [Authorize(Policy = "AllRole")]
        public async Task<IActionResult> EditUserProfile([FromBody] UpdateUserDTO updateUserDTO)
        {
    
            var result = await _authService.EditUserProfileAsnyc(updateUserDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpDelete("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid Id)
        {
           
            var result = await _authService.DeleteUserAsnyc(Id, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpDelete("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] RemoveRoleUserDTO removeRoleUserDTO)
        {
         
            var result = await _authService.RemoveRoleFromUserAsync(removeRoleUserDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
           

            var result = await _authService.RegisterAsync(registerDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpPatch("[action]")]
        [Authorize(Policy = "SuperAdminRole")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDTO assignRoleDTO)
        {
            
            var result = await _authService.AssignRoleToUserAsnyc(assignRoleDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChecekdConfirmedEmailToken([FromBody] ConfirmedEmailDTO confirmedEmailDTO)
        {
       
            var result = await _authService.ChecekdConfirmedEmailTokenAsnyc(confirmedEmailDTO.token, confirmedEmailDTO.Email, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> SendEmailTokenForForgotPassword([FromQuery] string Email)
        {
        
            var result = await _authService.SendEmailTokenForForgotPasswordAsync(Email, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckTokenForForgotPassword([FromQuery] string Email, [FromQuery] string Token)
        {
            
            var result = await _authService.CheckTokenForForgotPasswordAsync(Email, Token, _getRequestLangService.GetRequestLanguage());

            return StatusCode((int)result.StatusCode, result); ;
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangePasswordForTokenForgotPassword([FromBody] UpdateForgotPasswordDTO updateForgotPasswordDTO)
        {
          
            var result = await _authService.ChangePasswordForTokenForgotPasswordAsync(updateForgotPasswordDTO, _getRequestLangService.GetRequestLanguage());
            return StatusCode((int)result.StatusCode, result); ;
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("[action]")]
        public IActionResult GetUserRole()
        {
            var result = _authService.GetAllUserForSelect();
            return StatusCode((int)result.StatusCode, result);



        }
    }
}
