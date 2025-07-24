using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;


namespace Shop.Domain.Exceptions
{
   public class MultilanguageIdentityErrorDescriber:IdentityErrorDescriber
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultilanguageIdentityErrorDescriber(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private string LangCode =>
        _httpContextAccessor?.HttpContext?.Request?.Headers["Accept-Language"].ToString()?.ToLower() ?? "az";

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = nameof(ConcurrencyFailure),
                Description = AuthStatusException.ConcurrencyFailure[LangCode]
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = AuthStatusException.DefaultError[LangCode]
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = AuthStatusException.PasswordRequiresUniqueChars[LangCode].Replace("{0}", uniqueChars.ToString())
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyHasPassword),
                Description = AuthStatusException.UserAlreadyHasPassword[LangCode]
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = AuthStatusException.UserNameAlreadyExists[LangCode]
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = nameof(RecoveryCodeRedemptionFailed),
                Description = AuthStatusException.RecoveryCodeRedemptionFailed[LangCode]
            };
        }

        public override IdentityError DuplicateRoleName(string name)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateRoleName),
                Description = AuthStatusException.RoleAlreadyExists[LangCode]
            };
        }

        public override IdentityError DuplicateUserName(string name)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description =AuthStatusException.UserNameAlreadyExists[LangCode]
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = AuthStatusException.UserPasswordOrEmailWrong[LangCode]
            };
        }

        public override IdentityError InvalidRoleName(string name)
        {
            return new IdentityError
            {
                Code = nameof(InvalidRoleName),
                Description = AuthStatusException.RoleNotFound[LangCode]    
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = AuthStatusException.InvalidToken[LangCode]
            };
        }

        public override IdentityError InvalidUserName(string name)
        {
            return new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = AuthStatusException.InvalidUserName[LangCode].Replace("{0}", name)
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = nameof(LoginAlreadyAssociated),
                Description = AuthStatusException.LoginAlreadyAssociated[LangCode]
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = AuthStatusException.PasswordMismatch[LangCode]
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = AuthStatusException.PasswordRequiresDigit[LangCode]
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = AuthStatusException.PasswordRequiresLower[LangCode]
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = AuthStatusException.PasswordRequiresUpper[LangCode]
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = AuthStatusException.PasswordTooShort[LangCode].Replace("{0}", length.ToString())
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = AuthStatusException.PasswordRequiresNonAlphanumeric[LangCode]
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserAlreadyInRole),
                Description = AuthStatusException.UserAlreadyInRole[LangCode].Replace("{0}", role)
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = nameof(UserLockoutNotEnabled),
                Description = AuthStatusException.UserNotFound[LangCode]
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(UserNotInRole),
                Description = AuthStatusException.UserNotInRole[LangCode].Replace("{0}", role)
            };
        }
    }
}
