using Shop.Domain.Entities;

namespace Shop.Application.Abstraction.Token
{
   public interface ITokenHandler
    {
        Task<DTOs.Token> CreateAccessTokenAsync(User User, List<string> roles);
        string CreateRefreshToken();
    }
}
