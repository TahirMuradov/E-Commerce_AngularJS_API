using FluentValidation;
using Shop.Application.DTOs.PaymentMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.PaymentMethodValidations
{
   public class UpdatePaymentMethodDTOValidation:AbstractValidator<UpdatePaymentMethodDTO>
    {
        public UpdatePaymentMethodDTOValidation(string langCode, string[] SupportLanguages)
        {
            var culture = new CultureInfo(langCode);
            RuleFor(dto => dto.Id)
           .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdIsRequired", culture))
           .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidGuid", culture));

            RuleFor(dto => dto.Content)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangDictionaryIsRequired",culture));

            RuleForEach(dto => dto.Content)
                .Must(pair => !string.IsNullOrEmpty(pair.Key) && !string.IsNullOrEmpty(pair.Value))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangKeyAndValueRequired", culture));

            RuleForEach(dto => dto.Content.Keys)
                .Must(key => SupportLanguages.Contains(key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangKey", culture));
        }
    }
}
