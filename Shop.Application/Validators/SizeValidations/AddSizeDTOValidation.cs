using FluentValidation;
using Shop.Application.DTOs.SizeDTOs;
using System.Globalization;

namespace Shop.Application.Validators.SizeValidations
{
   public class AddSizeDTOValidation:AbstractValidator<AddSizeDTO>
    {
        public AddSizeDTOValidation()
        {

            RuleFor(dto => dto.Size)
               .NotEmpty()
                .WithMessage(GetTranslation("SizesRequired"))
               ;

      

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
