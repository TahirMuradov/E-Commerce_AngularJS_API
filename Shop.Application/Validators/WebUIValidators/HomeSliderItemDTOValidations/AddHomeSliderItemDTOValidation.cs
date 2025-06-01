using FluentValidation;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.HomeSliderItemDTOValidations
{
    public class AddHomeSliderItemDTOValidation: AbstractValidator<AddHomeSliderItemDTO>
    {
        public AddHomeSliderItemDTOValidation(string culture, string[] SupportedLaunguages)
        {
            CultureInfo cultureInfo = new CultureInfo(culture);

            // Validate that DescriptionContent is not null and contains at least three entries
            RuleFor(dto => dto.Description)
                .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo))
                .Must(langContent => langContent != null && langContent.Count == 3)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort",cultureInfo));

            // Validate that each key in DescriptionContent is a valid language code
            RuleFor(dto => dto.Description.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode",cultureInfo));


            // Validate that each value in DescriptionContent is not null or empty
            RuleForEach(dto => dto.Description.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo));
            // Validate that TitleContent is not null and contains at least three entries
            RuleFor(dto => dto.Title)
                .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo))
                .Must(langContent => langContent != null && langContent.Count == 3)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort",cultureInfo));

            // Validate that each key in TitleContent is a valid language code
            RuleFor(dto => dto.Title.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode",cultureInfo));


            // Validate that each value in TitleContent is not null or empty
            RuleForEach(dto => dto.Title.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo));

            //// Validate Pictures
            RuleFor(x => x.BackgroundImage)
                .NotNull()
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PictureİsRequired",cultureInfo));

        }
    }
}
