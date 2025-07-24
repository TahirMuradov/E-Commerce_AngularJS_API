using FluentValidation;
using Shop.Application.DTOs.SizeDTOs;
using System.Globalization;

namespace Shop.Application.Validators.SizeValidations
{
   public class UpdateSizeDTOValidation:AbstractValidator<UpdateSizeDTO>
    {
        public UpdateSizeDTOValidation()
        {
   

            RuleFor(dto => dto.Size)
               .NotNull()
               .WithMessage(GetTranslation("SizesRequired"));
            RuleFor(dto => dto.Id)
               .Must(langContent => langContent != default)
               .WithMessage(GetTranslation("IdInvalid"))
               .NotEmpty()
               .WithMessage(GetTranslation("IdRequired"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
