using FluentValidation;
using Shop.Application.DTOs.ShippingMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ShippingMethodValidations
{
  public  class UpdateShippingMethodDTOValidation:AbstractValidator<UpdateShippingMethodDTO>
    {
        public UpdateShippingMethodDTOValidation(string LangCode,string[] SupportLanguages)
        {
            var culture = new CultureInfo(LangCode);
            RuleFor(dto => dto.Id)
                .Must(id => id != default)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdInvalid", culture));

            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangDictionaryIsRequired", culture));

            RuleForEach(dto => dto.Content)
                .Must(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangKeyAndValueRequired", culture));

            RuleForEach(dto => dto.Content.Keys)
                .Must(key =>    SupportLanguages.Contains(key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangKey",culture));
        }
    }
}
