using FluentValidation;
using Shop.Application.DTOs.PaymentMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.PaymentMethodValidations
{
   public class UpdatePaymentMethodDTOValidation:AbstractValidator<UpdatePaymentMethodDTO>
    {
        public UpdatePaymentMethodDTOValidation( string[] SupportLanguages)
        {
          ;
            RuleFor(dto => dto.Id)
           .NotEmpty().WithMessage(GetTranslation("IdIsRequired"))
           .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage(GetTranslation("InvalidGuid"));

            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(GetTranslation("LangDictionaryIsRequired"));

            RuleForEach(dto => dto.Content)
                .Must(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .WithMessage(GetTranslation("LangKeyAndValueRequired"));

            RuleForEach(dto => dto.Content.Keys)
                .Must(key => SupportLanguages.Contains(key))
                .WithMessage(GetTranslation("InvalidLangKey"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
