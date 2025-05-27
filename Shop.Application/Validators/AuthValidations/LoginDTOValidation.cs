using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Validators.AuthValidations
{
  public  class LoginDTOValidation:AbstractValidator<LoginDTO>
    {
        public LoginDTOValidation(string LangCode)
        {

            var culture = new CultureInfo(LangCode);
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailRequired", culture ))
                .EmailAddress().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("EmailInvalid", culture));

           
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PasswordRequired",culture));

        }
    }
}
