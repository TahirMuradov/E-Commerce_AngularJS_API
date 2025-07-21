using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
   public class RegisterDTOValidation:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidation()
        {
          
            // Firstname validation: not null or empty
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage(GetTranslation("FirstnameRequired"));

            // Lastname validation: not null or empty
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage(GetTranslation("LastnameRequired"));

            // Email validation: not null or empty
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(GetTranslation("EmailRequired"))
                .EmailAddress().WithMessage(GetTranslation("EmailInvalid"));

            // PhoneNumber validation: +994-xx-xxx-xx-xx||xxx-xxx-xx-xx format
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(    GetTranslation("PhoneNumberRequired"))
                .Matches(@"^(?:\+994-?(?:\d{2}-?\d{3}-?\d{2}-?\d{2}|\d{2}-?\d{3}-?\d{2}-?\d{2})|(\d{3}-?\d{3}-?\d{2}-?\d{2}|\d{3}-?\d{3}-?\d{2}-?\d{2}-?))$")
                .WithMessage(GetTranslation("PhoneNumberInvalid"));

            // Address validation: not null or empty
            RuleFor(x => x.Adress)
                .NotEmpty().WithMessage(GetTranslation("AddressRequired"));

            // Username validation: not null or empty
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(GetTranslation("UsernameRequired"));

            // Password validation: not null or empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(GetTranslation("PasswordRequired"));

            // ConfirmPassword validation: not null or empty and must match Password
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(GetTranslation("ConfirmPasswordRequired"))
                .Equal(x => x.Password).WithMessage(GetTranslation("PasswordsDoNotMatch"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
