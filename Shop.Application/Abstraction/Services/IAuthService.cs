using Shop.Application.DTOs.AuthDTOs;
using Shop.Application.PaginationHelper;
using Shop.Application.ResultTypes.Abstract;
using Shop.Application.Validators.AuthValidations;
using Shop.Domain.Entities;


namespace Shop.Application.Abstraction.Services
{
   public interface IAuthService
    {
        Task<IDataResult<PaginatedList<GetUserDTO>>> GetAllUserByPageOrSearchAsync(int page,string?search=null);
        Task<IResult> ChecekdConfirmedEmailTokenAsnyc(string email, string token, string culture);
        Task<IResult> EditUserProfileAsnyc(UpdateUserDTO updateUserDTO, string culture);
        Task<IResult> DeleteUserAsnyc(Guid Id, string culture);
        Task<IResult> RegisterAsync(RegisterDTO registerDTO, string culture);
        Task<IResult> AssignRoleToUserAsnyc(AssignRoleDTO assignRoleDTO, string culture);
        Task<IResult> SendEmailTokenForForgotPasswordAsync(string Email,string LangCode);
        Task<IResult> CheckTokenForForgotPasswordAsync(string Email, string token,string LangCode);
        Task<IResult> ChangePasswordForTokenForgotPasswordAsync(UpdateForgotPasswordDTO updateForgotPasswordDTO,string LangCode);
        IDataResult<IQueryable<GetUserForSelectDTO>> GetAllUserForSelect();
        Task<IDataResult<string>> UpdateRefreshTokenAsnyc(string refreshToken, User user, string culture);
        Task<IResult> RemoveRoleFromUserAsync(RemoveRoleUserDTO removeRoleUserDTO, string culture);
        
        Task<IDataResult<DTOs.Token>> LoginAsync(LoginDTO loginDTO, string culture);
        Task<IResult> LogOutAsync(string userId, string culture);
    }
}
