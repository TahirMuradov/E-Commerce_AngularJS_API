using FluentValidation;
using Shop.Application.DTOs.ShippingMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ShippingMethodValidations
{
  public  class UpdateShippingMethodDTOValidation:AbstractValidator<UpdateShippingMethodDTO>
    {
        public UpdateShippingMethodDTOValidation(string[] SupportLanguages)
        {
    
            RuleFor(dto => dto.Id)
                .Must(id => id != default)
                .WithMessage(GetTranslation("IdInvalid"));

            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(GetTranslation("LangDictionaryIsRequired"));

            RuleForEach(dto => dto.Content)
                .Must(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .WithMessage(GetTranslation("LangKeyAndValueRequired"));

            RuleForEach(dto => dto.Content.Keys)
                .Must(key =>    SupportLanguages.Contains(key))
                .WithMessage(GetTranslation("InvalidLangKey"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
