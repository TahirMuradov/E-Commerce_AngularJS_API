using FluentValidation;
using Shop.Application.DTOs.ShippingMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ShippingMethodValidations
{
    public class AddShippingMethodDTOValidation : AbstractValidator<AddShippingMethodDTO>
    {
        public AddShippingMethodDTOValidation(string langCode, string[] SupportLanguages)
        {
            var culture = new CultureInfo(langCode);
            RuleFor(x => x.DisCountPrice)
              .GreaterThanOrEqualTo(0).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("DisCountNegativeNumberCheck", culture));

            RuleFor(x => x.Price)
               .GreaterThan(0).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PriceNegativeNumberCheck", culture));

            // Validate that LangContent is not null and contains at least three entries
            RuleFor(dto => dto.Content)
                .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", culture))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            // Validate that each key in LangContent is a valid language code
            RuleFor(dto => dto.Content.Keys)
       .Must(keys => keys.All(key => (SupportLanguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture));


            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.Content.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", culture));
        }
    }
}
