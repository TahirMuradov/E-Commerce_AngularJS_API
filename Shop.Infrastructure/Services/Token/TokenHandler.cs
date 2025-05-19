using Shop.Application.Abstraction.Token;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        public Task<Application.DTOs.Token> CreateAccessTokenAsync(User User, List<string> roles)
        {
            throw new NotImplementedException();
        }

        public string CreateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
