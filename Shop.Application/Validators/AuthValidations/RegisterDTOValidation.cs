using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
   public class RegisterDTOValidation:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidation(string LangCode)
        {
            // Firstname validation: not null or empty
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("FirstnameRequired", new CultureInfo(LangCode)));

            // Lastname validation: not null or empty
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LastnameRequired", new CultureInfo(LangCode)));

            // Email validation: not null or empty
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailRequired", new CultureInfo(LangCode)))
                .EmailAddress().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailInvalid", new CultureInfo(LangCode)));

            // PhoneNumber validation: +994-xx-xxx-xx-xx||xxx-xxx-xx-xx format
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberRequired", new CultureInfo(LangCode)))
                .Matches(@"^(?:\+994-?(?:\d{2}-?\d{3}-?\d{2}-?\d{2}|\d{2}-?\d{3}-?\d{2}-?\d{2})|(\d{3}-?\d{3}-?\d{2}-?\d{2}|\d{3}-?\d{3}-?\d{2}-?\d{2}-?))$")
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberInvalid", new CultureInfo(LangCode)));

            // Address validation: not null or empty
            RuleFor(x => x.Adress)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("AddressRequired", new CultureInfo(LangCode)));

            // Username validation: not null or empty
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UsernameRequired", new CultureInfo(LangCode)));

            // Password validation: not null or empty
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordRequired", new CultureInfo(LangCode)));

            // ConfirmPassword validation: not null or empty and must match Password
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ConfirmPasswordRequired", new CultureInfo(LangCode)))
                .Equal(x => x.Password).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordsDoNotMatch", new CultureInfo(LangCode)));
        }

    }
}
