using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
    public class UpdateForgotPasswordDTOValidation:AbstractValidator<UpdateForgotPasswordDTO>
    {
        public UpdateForgotPasswordDTOValidation(string LangCode)
        {

            RuleFor(x => x.Email)
         .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailRequired", new CultureInfo(LangCode)))
         .EmailAddress().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailInvalid", new CultureInfo(LangCode)));

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("TokenRequired", new CultureInfo(LangCode)));

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordRequired", new CultureInfo(LangCode)));
           

            RuleFor(x => x.NewConfirmPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ConfirmPasswordRequired", new CultureInfo(LangCode)))
                .Equal(x => x.NewPassword).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordsDoNotMatch", new CultureInfo(LangCode)));


    ;
        }
    }
}
