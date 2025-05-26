using FluentValidation;
using Shop.Application.DTOs.SizeDTOs;
using System.Globalization;

namespace Shop.Application.Validators.SizeValidations
{
   public class UpdateSizeDTOValidation:AbstractValidator<UpdateSizeDTO>
    {
        public UpdateSizeDTOValidation(string langCode)
        {

            RuleFor(dto => dto.Size)
               .NotNull()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("SizesRequired", new CultureInfo(langCode)));
            RuleFor(dto => dto.Id)
               .Must(langContent => langContent != default)
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdInvalid", new CultureInfo(langCode)))
               .NotEmpty()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdRequired", new CultureInfo(langCode)));


        }
    }
}
