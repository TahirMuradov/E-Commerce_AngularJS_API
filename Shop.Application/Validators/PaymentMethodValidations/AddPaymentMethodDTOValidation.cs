using FluentValidation;
using Shop.Application.DTOs.PaymentMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.PaymentMethodValidations
{
   public class AddPaymentMethodDTOValidation:AbstractValidator<AddPaymentMethodDTO>
    {
        public AddPaymentMethodDTOValidation( string[] SupportLanguages)
        {

            RuleFor(dto => dto.Content)
               .NotNull()
               .WithMessage(GetTranslation("ContentEmpty"))
               .Must(langContent => langContent != null && langContent.Count > 3)
               .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in LangContent is a valid language code
            RuleForEach(dto => dto.Content.Keys)
                .Must(key => SupportLanguages.Contains(key))
                .WithMessage(GetTranslation("InvalidLangCode"));

            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.Content.Values)
                .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
