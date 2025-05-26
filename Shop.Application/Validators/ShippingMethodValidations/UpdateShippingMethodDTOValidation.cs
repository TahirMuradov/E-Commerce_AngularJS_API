using FluentValidation;
using Shop.Application.DTOs.ShippingMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ShippingMethodValidations
{
  public  class UpdateShippingMethodDTOValidation:AbstractValidator<UpdateShippingMethodDTO>
    {
        public UpdateShippingMethodDTOValidation(string LangCode,string[] SupportLanguages)
        {
            RuleFor(dto => dto.Id)
                .Must(id => id != default)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdInvalid", new CultureInfo(LangCode)));

            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangDictionaryIsRequired", new CultureInfo(LangCode)));

            RuleForEach(dto => dto.Content)
                .Must(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangKeyAndValueRequired", new CultureInfo(LangCode)));

            RuleForEach(dto => dto.Content.Keys)
                .Must(key =>    SupportLanguages.Contains(key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangKey", new CultureInfo(LangCode)));
        }
    }
}
