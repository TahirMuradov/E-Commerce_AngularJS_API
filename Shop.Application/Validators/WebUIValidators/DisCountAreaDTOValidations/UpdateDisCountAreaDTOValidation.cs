using FluentValidation;
using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.DisCountAreaDTOValidations
{
  public  class UpdateDisCountAreaDTOValidation : AbstractValidator<UpdateDisCountAreaDTO>
    {
        public UpdateDisCountAreaDTOValidation(string culture, string[] SupportedLaunguages)
        {

            CultureInfo cultureInfo = new CultureInfo(culture);



            RuleFor(dto => dto.Id)
            .NotEmpty()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdIsRequired",cultureInfo))
           .NotNull()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdIsRequired",cultureInfo));
            // Validate that DescriptionContent is not null and contains at least three entries
            RuleFor(dto => dto.DescriptionContent)
                .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo))
                .Must(langContent => langContent != null && langContent.Count == 3)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort",cultureInfo));

            // Validate that each key in DescriptionContent is a valid language code
            RuleFor(dto => dto.DescriptionContent.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode",cultureInfo));


            // Validate that each value in DescriptionContent is not null or empty
            RuleForEach(dto => dto.DescriptionContent.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo));
            // Validate that TitleContent is not null and contains at least three entries
            RuleFor(dto => dto.TitleContent)
                .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo))
                .Must(langContent => langContent != null && langContent.Count == 3)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort",cultureInfo));

            // Validate that each key in TitleContent is a valid language code
            RuleFor(dto => dto.TitleContent.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode",cultureInfo));


            // Validate that each value in TitleContent is not null or empty
            RuleForEach(dto => dto.TitleContent.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo));

        }
    }
}
