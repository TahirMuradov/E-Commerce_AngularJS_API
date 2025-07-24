using Microsoft.AspNetCore.Identity;

namespace Shop.Domain.Entities
{
   public class User:IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Adress { get; set; }


        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
    }
}
