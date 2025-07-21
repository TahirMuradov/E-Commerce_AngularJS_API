using FluentValidation;
using Shop.Application.DTOs.ShippingMethodDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ShippingMethodValidations
{
    public class AddShippingMethodDTOValidation : AbstractValidator<AddShippingMethodDTO>
    {
        public AddShippingMethodDTOValidation( string[] SupportLanguages)
        {
          
            RuleFor(x => x.DisCountPrice)
              .GreaterThanOrEqualTo(0).WithMessage(GetTranslation("DisCountNegativeNumberCheck"));

            RuleFor(x => x.Price)
               .GreaterThan(0).WithMessage(GetTranslation("PriceNegativeNumberCheck"));

            // Validate that LangContent is not null and contains at least three entries
            RuleFor(dto => dto.Content)
                .NotNull()
                .WithMessage(GetTranslation("ContentEmpty"))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in LangContent is a valid language code
            RuleFor(dto => dto.Content.Keys)
       .Must(keys => keys.All(key => (SupportLanguages).Contains(key)))
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
