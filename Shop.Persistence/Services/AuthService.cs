
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shop.Application.Abstraction.Services;
using Shop.Application.Abstraction.Token;
using Shop.Application.DTOs;
using Shop.Application.DTOs.AuthDTOs;
using Shop.Application.Exceptions;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.ResultTypes.Concrete.ErrorResults;
using Shop.Application.ResultTypes.Concrete.SuccessResults;
using Shop.Application.Validators.AuthValidations;
using Shop.Domain.Entities;
using Shop.Persistence;
using System.Net;

namespace Shop.Infrastructure
{
    public class AuthService : IAuthServices
    {
        private string[] SupportedLaunguages
        {
            get
            {



                return Configuration.config.GetSection("SupportedLanguage:Launguages").Get<string[]>();


            }
        }

        private string DefaultLaunguage
        {
            get
            {
                return Configuration.config.GetSection("SupportedLanguage:Default").Get<string>();
            }
        }
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IMailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, ITokenHandler tokenHandler, IMailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenHandler = tokenHandler;
            _emailService = emailService;
            _configuration = configuration;
        }

        public Task<IResult> AssignRoleToUserAsnyc(AssignRoleDTO assignRoleDTO, string culture)
        {
            throw new NotImplementedException();

        }

        public Task<IResult> ChangePasswordForTokenForgotPassword(string Email, string token, string NewPassword)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> ChecekdConfirmedEmailTokenAsnyc(string email, string token, string culture)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> CheckTokenForForgotPassword(string Email, string token)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteUserAsnyc(Guid Id, string culture)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> EditUserProfileAsnyc(UpdateUserDTO updateUserDTO, string culture)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PaginatedList<GetAllUserDTO>>> GetAllUserAsnyc(int page)
        {
            throw new NotImplementedException();
        }

        public IDataResult<IQueryable<GetAllUserForSelectDTO>> GetAllUserForSelect()
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<Token>> LoginAsync(LoginDTO loginDTO, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            LoginDTOValidation validationRules = new LoginDTOValidation(culture);
            var validationResult = await validationRules.ValidateAsync(loginDTO);
            if (!validationResult.IsValid)
                return new ErrorDataResult<Token>(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            User user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user is  null)
                return new ErrorDataResult<Token>(message: AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = await _tokenHandler.CreateAccessTokenAsync(user, roles.ToList());
                var refreshToken = _tokenHandler.CreateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = DateTime.UtcNow.AddDays(15);
                await _userManager.UpdateAsync(user);
                return new SuccessDataResult<Token>(new Token
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    Expiration = token.Expiration
                }, HttpStatusCode.OK);
            }
            return new ErrorDataResult<Token>(message: AuthStatusException.UserPasswordOrEmailWrong[culture], HttpStatusCode.BadRequest);

        }

        public async Task<IResult> LogOutAsync(string userId, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            if (string.IsNullOrEmpty(userId)) return new ErrorResult(statusCode: HttpStatusCode.NotFound, message: AuthStatusException.UserNotFound[culture]);

            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser == null)
                return new ErrorResult(statusCode: HttpStatusCode.NotFound, message: AuthStatusException.UserNotFound[culture]);


            findUser.RefreshToken = null;
            findUser.RefreshTokenEndDate = null;
            var result = await _userManager.UpdateAsync(findUser);
            await _signInManager.SignOutAsync();
            if (result.Succeeded)
            {
                return new SuccessResult(statusCode: HttpStatusCode.OK);
            }
            else
            {
             
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, messages: result.Errors.Select(x=>x.Description).ToList());
            }
        }

        public Task<IResult> RegisterAsync(RegisterDTO registerDTO, string culture)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RemoveRoleFromUserAsync(RemoveRoleUserDTO removeRoleUserDTO, string culture)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SendEmailTokenForForgotPassword(string Email)
        {
            throw new NotImplementedException();
        }
    }
}
