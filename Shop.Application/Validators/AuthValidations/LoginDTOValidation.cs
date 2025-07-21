using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
  public  class LoginDTOValidation:AbstractValidator<LoginDTO>
    {
        public LoginDTOValidation()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(GetTranslation("EmailRequired"))
                .EmailAddress().WithMessage(GetTranslation("EmailInvalid"));

           
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(GetTranslation("PasswordRequired"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
