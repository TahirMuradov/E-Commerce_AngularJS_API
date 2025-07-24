using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
    public class UpdateForgotPasswordDTOValidation:AbstractValidator<UpdateForgotPasswordDTO>
    {
        public UpdateForgotPasswordDTOValidation()
        {

            RuleFor(x => x.Email)
         .NotEmpty().WithMessage(GetTranslation("EmailRequired"))
         .EmailAddress().WithMessage(GetTranslation("EmailInvalid"));

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage(GetTranslation("TokenRequired"));

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(GetTranslation("PasswordRequired"));
           

            RuleFor(x => x.NewConfirmPassword)
                .NotEmpty().WithMessage(GetTranslation("ConfirmPasswordRequired"))
                .Equal(x => x.NewPassword).WithMessage(GetTranslation("PasswordsDoNotMatch"));


    ;
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
