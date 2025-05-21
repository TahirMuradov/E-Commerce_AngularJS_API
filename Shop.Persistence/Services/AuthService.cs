using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using System.Web;

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

        public async Task<IResult> AssignRoleToUserAsnyc(AssignRoleDTO assignRoleDTO, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            AssignRoleDTOValidator validationRules = new AssignRoleDTOValidator(culture);
            var validationResult = await validationRules.ValidateAsync(assignRoleDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), statusCode: HttpStatusCode.BadRequest);

            User user = await _userManager.FindByIdAsync(assignRoleDTO.UserId.ToString());
            string responseMessage = string.Empty;
            if (user == null)
                return new ErrorResult(AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);
            else
            {
                Role role = await _roleManager.FindByIdAsync(assignRoleDTO.RoleId.ToString());
                if (role == null)
                    return new ErrorResult(AuthStatusException.RoleNotFound[culture], HttpStatusCode.NotFound);
                IdentityResult identityResult = await _userManager.AddToRoleAsync(user, role.Name);
                if (!identityResult.Succeeded)
                    return new ErrorResult(messages: identityResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);


                return new SuccessResult(HttpStatusCode.OK);

            }
        }

        public async Task<IResult> ChangePasswordForTokenForgotPasswordAsync(string Email, string token, string NewPassword, string LangCode)
        {
            if (string.IsNullOrWhiteSpace(LangCode) || !SupportedLaunguages.Contains(LangCode))
                LangCode = DefaultLaunguage;
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(NewPassword))
                return new ErrorResult(HttpStatusCode.BadRequest);
            Email = HttpUtility.UrlDecode(Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null)
                return new ErrorResult(message: AuthStatusException.UserNotFound[LangCode], HttpStatusCode.NotFound);

            token = HttpUtility.UrlDecode(token);
            NewPassword = HttpUtility.UrlDecode(NewPassword);
            IdentityResult tokenResult = await _userManager.ResetPasswordAsync(user, token, NewPassword);
            if (tokenResult.Succeeded)
                return new SuccessResult(HttpStatusCode.OK);
            return new ErrorResult(messages: tokenResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);


        }

        public async Task<IResult> ChecekdConfirmedEmailTokenAsnyc(string email, string token, string culture)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token)) return new ErrorResult(HttpStatusCode.BadRequest);
            if (string.IsNullOrEmpty(culture) || !SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            var checekedEmail = await _userManager.FindByEmailAsync(email);
            if (checekedEmail is null) return new ErrorResult(message: AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);

            if (checekedEmail.EmailConfirmed)
                return new ErrorResult(HttpStatusCode.BadRequest);
            IdentityResult checekedResult = await _userManager.ConfirmEmailAsync(checekedEmail, token);
            if (checekedResult.Succeeded)

                return new SuccessResult(messages: checekedResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.OK);

            else
                return new ErrorResult(messages: checekedResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);


        }

        public async Task<IResult> CheckTokenForForgotPasswordAsync(string Email, string token, string LangCode)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(token)) return new ErrorResult(HttpStatusCode.BadRequest);

            Email = HttpUtility.UrlDecode(Email);
            token = HttpUtility.UrlDecode(token);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null)
                return new ErrorResult(AuthStatusException.UserNotFound[LangCode], HttpStatusCode.NotFound);
            bool tokenResult = await _userManager.VerifyUserTokenAsync(
      user: user,
       tokenProvider: _userManager.Options.Tokens.PasswordResetTokenProvider,
     purpose: UserManager<User>.ResetPasswordTokenPurpose,
    token: token
                  );

            if (tokenResult) return new SuccessResult(HttpStatusCode.OK);
            return new ErrorResult(HttpStatusCode.BadRequest);
        }

        public async Task<IResult> DeleteUserAsnyc(Guid Id, string culture)
        {

            User ChecekdUSerId = await _userManager.FindByIdAsync(Id.ToString());
            if (ChecekdUSerId == null) return new ErrorResult(message: AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);
            IdentityResult result = await _userManager.DeleteAsync(ChecekdUSerId);
            if (result.Succeeded)
                return new SuccessResult(HttpStatusCode.OK);
            else
                return new ErrorResult(messages: result.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);


        }

        public async Task<IResult> EditUserProfileAsnyc(UpdateUserDTO updateUserDTO, string culture)
        {
            if (string.IsNullOrEmpty(culture) || SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            var checekedUser = await _userManager.FindByIdAsync(updateUserDTO.UserId.ToString());
            if (checekedUser is null) return new ErrorResult(message: AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);
            if (!string.IsNullOrEmpty(updateUserDTO.Firstname) && checekedUser.FirstName != updateUserDTO.Firstname)
                checekedUser.FirstName = updateUserDTO.Firstname;
            if (!string.IsNullOrEmpty(updateUserDTO.Username) && checekedUser.UserName != updateUserDTO.Username)
                checekedUser.UserName = updateUserDTO.Username;
            if (!string.IsNullOrEmpty(updateUserDTO.Lastname) && checekedUser.LastName != updateUserDTO.Lastname)
                checekedUser.LastName = updateUserDTO.Lastname;
            if (!string.IsNullOrEmpty(updateUserDTO.Adress) && checekedUser.Adress != updateUserDTO.Adress)
                checekedUser.Adress = updateUserDTO.Adress;
            if (!string.IsNullOrEmpty(updateUserDTO.PhoneNumber) && checekedUser.PhoneNumber != updateUserDTO.PhoneNumber)
                checekedUser.PhoneNumber = updateUserDTO.PhoneNumber;
            if (!string.IsNullOrEmpty(updateUserDTO.CurrentPassword) && !string.IsNullOrEmpty(updateUserDTO.NewPassword))
            {

                IdentityResult changePassword = await _userManager.ChangePasswordAsync(checekedUser, updateUserDTO.CurrentPassword, updateUserDTO.NewPassword);
                if (!changePassword.Succeeded)
                    return new ErrorResult(messages: changePassword.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);
            }
            IdentityResult UpdateUserResult = await _userManager.UpdateAsync(checekedUser);


            return UpdateUserResult.Succeeded ? new SuccessResult(HttpStatusCode.OK) :
                  new ErrorResult(messages: UpdateUserResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);


        }

        public async Task<IDataResult<PaginatedList<GetAllUserDTO>>> GetAllUserAsnyc(int page)
        {
            if (page < 1)
                page = 1;
            var usersQuery = _userManager.Users.AsNoTracking();
            List<GetAllUserDTO> result=new();
            foreach (var user in usersQuery)
            {
                var roles = await _userManager.GetRolesAsync(user);

                result.Add(new GetAllUserDTO
                {
                    Id = user.Id,
                    Adress = user.Adress,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = roles.ToList()
                });
            }

            var paginatedList = PaginatedList<GetAllUserDTO>.Create(result, page, 10);







           
            return new SuccessDataResult<PaginatedList<GetAllUserDTO>>(data: paginatedList, HttpStatusCode.OK);
        }

        public IDataResult<IQueryable<GetAllUserForSelectDTO>> GetAllUserForSelect()
        {
            return new SuccessDataResult<IQueryable<GetAllUserForSelectDTO>>(_userManager.Users.Select(x => new GetAllUserForSelectDTO
            {
                Userid = x.Id,
                Email = x.Email
            }), HttpStatusCode.OK);
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
            if (user is null)
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

                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, messages: result.Errors.Select(x => x.Description).ToList());
            }
        }

        public async Task<IResult> RegisterAsync(RegisterDTO registerDTO, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            RegisterDTOValidation validationRules = new RegisterDTOValidation(culture);
            var validationResult = await validationRules.ValidateAsync(registerDTO);
            if (!validationResult.IsValid) return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
            var checkEmail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == registerDTO.Email);
            var checkUserName = await _userManager.FindByNameAsync(registerDTO.Username);

            if (checkEmail != null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: AuthStatusException.EmailAlreadyExists[culture]);

            if (checkUserName != null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: AuthStatusException.UserNameAlreadyExists[culture]);

            User newUser = new()
            {
                FirstName = registerDTO.Firstname,
                LastName = registerDTO.Lastname,
                Email = registerDTO.Email,
                UserName = registerDTO.Username,
                PhoneNumber = registerDTO.PhoneNumber,
                Adress = registerDTO.Adress,


            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerDTO.Password);

            if (identityResult.Succeeded)
            {
                var users = _userManager.Users.AsNoTracking();
                var roles = _roleManager.Roles.AsNoTracking();
                if (users.Count()==1)
                {
                    if (roles.Count()==0)
                    {

                        Role Role = new Role()
                        {
                            Name = "SuperAdmin"
                        };
                        Role Role1 = new Role()
                        {
                            Name = "Admin"
                        };
                        Role Role2 = new Role()
                        {
                            Name = "User"
                        };
                        await _roleManager.CreateAsync(Role);
                        await _roleManager.CreateAsync(Role1);
                        await _roleManager.CreateAsync(Role2);
                    }
                    await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
                }
                else
                {
                    if (roles.Any(x=>x.Name=="User"))
                    {
                     
                      await _userManager.AddToRoleAsync(newUser, "User");
                    }
                    else
                    {
                        Role Role = new Role()
                        {
                            Name = "User"
                        };
                        await _roleManager.CreateAsync(Role);
                        await _userManager.AddToRoleAsync(newUser, "User");
                    }
                    await _userManager.AddToRoleAsync(newUser, "User");
                }
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                string confimationLink = $"{Configuration.config.GetSection("Domain:Front").Get<string>()}/{culture}/auth/emailconfirmed/{HttpUtility.UrlEncode(newUser.Email)}/{HttpUtility.UrlEncode(token)}";
                var resultEmail = await _emailService.SendEmailAsync(newUser.Email, confimationLink, newUser.FirstName + " " + newUser.LastName);
                if (!resultEmail.IsSuccess)
                {
                    await _userManager.DeleteAsync(await _userManager.FindByEmailAsync(newUser.Email));
                    return new ErrorResult(message: AuthStatusException.ConfirmationLinkNotSend[culture], HttpStatusCode.BadRequest);
                }
                return new SuccessResult(message: AuthStatusException.RegistrationSuccess[culture], statusCode: HttpStatusCode.Created);
            }
            else
            {


                return new ErrorResult(messages: identityResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);
            }
        }

        public async Task<IResult> RemoveRoleFromUserAsync(RemoveRoleUserDTO removeRoleUserDTO, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;
            RemoveRoleUserDTOValidation validationRules = new RemoveRoleUserDTOValidation(culture);
            var validationResult = await validationRules.ValidateAsync(removeRoleUserDTO);
            if (!validationResult.IsValid)
                return new ErrorResult(messages: validationResult.Errors.Select(x => x.ErrorMessage).ToList(), HttpStatusCode.BadRequest);


            User user = await _userManager.FindByIdAsync(removeRoleUserDTO.UserId.ToString());

            if (user is null)
                return new ErrorResult(AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);
            else
            {
                foreach (var roleid in removeRoleUserDTO.RoleId)
                {
                    Role role = await _roleManager.FindByIdAsync(roleid.ToString());
                    if (role == null)
                        return new ErrorResult(AuthStatusException.RoleNotFound[culture], HttpStatusCode.NotFound);
                    IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    if (!identityResult.Succeeded)
                    {

                        return new ErrorResult(messages: identityResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);
                    }
                }


                return new SuccessResult(HttpStatusCode.OK);
            }
        }

        public async Task<IResult> SendEmailTokenForForgotPasswordAsync(string Email, string LangCode)
        {
            if (string.IsNullOrEmpty(Email) && !Email.Contains("@")) return new ErrorResult(AuthStatusException.EmailInvalid[LangCode], HttpStatusCode.BadRequest);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null)
                return new ErrorResult(message: AuthStatusException.UserNotFound[LangCode], HttpStatusCode.NotFound);
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var url = _configuration["Domain:Front"] + $"/auth/forgotpassword/confirmation/{HttpUtility.UrlEncode(Email)}/{HttpUtility.UrlEncode(token)}";
            var emailResult = await _emailService.SendEmailAsync(user.Email, url, user.FirstName + " " + user.LastName);


            if (emailResult.IsSuccess)

                return new SuccessResult(HttpStatusCode.OK);

            return new ErrorResult(HttpStatusCode.BadRequest);
        }

        public async Task<IDataResult<string>> UpdateRefreshTokenAsnyc(string refreshToken, User user, string culture)
        {
            if (!SupportedLaunguages.Contains(culture))
                culture = DefaultLaunguage;


            if (user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = DateTime.UtcNow.AddMonths(1);

                IdentityResult identityResult = await _userManager.UpdateAsync(user);

                if (identityResult.Succeeded)
                    return new SuccessDataResult<string>(statusCode: HttpStatusCode.OK, data: refreshToken);
                else
                    return new ErrorDataResult<string>(messages: identityResult.Errors.Select(x => x.Description).ToList(), HttpStatusCode.BadRequest);

            }
            else
                return new ErrorDataResult<string>(AuthStatusException.UserNotFound[culture], HttpStatusCode.NotFound);

        }
    }
}
