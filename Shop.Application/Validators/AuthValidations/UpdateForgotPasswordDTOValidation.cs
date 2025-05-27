using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
    public class UpdateForgotPasswordDTOValidation:AbstractValidator<UpdateForgotPasswordDTO>
    {
        public UpdateForgotPasswordDTOValidation(string LangCode)
        {
            var culture = new CultureInfo(LangCode);
            RuleFor(x => x.Email)
         .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailRequired", culture))
         .EmailAddress().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailInvalid", culture));

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("TokenRequired", culture));

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordRequired",culture));
           

            RuleFor(x => x.NewConfirmPassword)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ConfirmPasswordRequired", culture))
                .Equal(x => x.NewPassword).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordsDoNotMatch", culture));


    ;
        }
    }
}
