using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
   public class RegisterDTOValidation:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidation(string LangCode)
        {
            var culture = new CultureInfo(LangCode);
            // Firstname validation: not null or empty
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("FirstnameRequired",culture));

            // Lastname validation: not null or empty
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LastnameRequired", culture));

            // Email validation: not null or empty
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailRequired", culture))
                .EmailAddress().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailInvalid",culture));

            // PhoneNumber validation: +994-xx-xxx-xx-xx||xxx-xxx-xx-xx format
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberRequired", culture))
                .Matches(@"^(?:\+994-?(?:\d{2}-?\d{3}-?\d{2}-?\d{2}|\d{2}-?\d{3}-?\d{2}-?\d{2})|(\d{3}-?\d{3}-?\d{2}-?\d{2}|\d{3}-?\d{3}-?\d{2}-?\d{2}-?))$")
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberInvalid", new CultureInfo(LangCode)));

            // Address validation: not null or empty
            RuleFor(x => x.Adress)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("AddressRequired", culture));

            // Username validation: not null or empty
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UsernameRequired", culture));

            // Password validation: not null or empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordRequired",culture));

            // ConfirmPassword validation: not null or empty and must match Password
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ConfirmPasswordRequired", culture))
                .Equal(x => x.Password).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordsDoNotMatch", culture));
        }

    }
}
