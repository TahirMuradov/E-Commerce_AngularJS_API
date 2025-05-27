using FluentValidation;
using Shop.Application.DTOs.PaymentMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.PaymentMethodValidations
{
   public class AddPaymentMethodDTOValidation:AbstractValidator<AddPaymentMethodDTO>
    {
        public AddPaymentMethodDTOValidation(string langCode, string[] SupportLanguages)
        {
            var culture = new CultureInfo(langCode);
            RuleFor(dto => dto.Content)
               .NotNull()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", culture))
               .Must(langContent => langContent != null && langContent.Count == 3)
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            // Validate that each key in LangContent is a valid language code
            RuleForEach(dto => dto.Content.Keys)
                .Must(key => SupportLanguages.Contains(key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture));

            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.Content.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", culture));
        }
    }
}
