using FluentValidation;
using Shop.Application.DTOs.SizeDTOs;
using System.Globalization;

namespace Shop.Application.Validators.SizeValidations
{
   public class AddSizeDTOValidation:AbstractValidator<AddSizeDTO>
    {
        public AddSizeDTOValidation(string langCode)
        {

            RuleFor(dto => dto.Size)
               .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("SizesRequired", new CultureInfo(langCode)))
               ;

      

        }
    }
}
