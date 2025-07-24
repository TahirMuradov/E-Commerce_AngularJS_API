using FluentValidation;
using Shop.Application.DTOs.WebUI.DisCountAreaDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.DisCountAreaDTOValidations
{
  public  class UpdateDisCountAreaDTOValidation : AbstractValidator<UpdateDisCountAreaDTO>
    {
        public UpdateDisCountAreaDTOValidation( string[] SupportedLaunguages)
        {




            RuleFor(dto => dto.Id)
            .NotEmpty()
               .WithMessage(GetTranslation("IdIsRequired"))
           .NotNull()
               .WithMessage(GetTranslation("IdIsRequired"));
            // Validate that DescriptionContent is not null and contains at least three entries
            RuleFor(dto => dto.DescriptionContent)
                .NotNull()
                .WithMessage(GetTranslation("ContentEmpty"))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in DescriptionContent is a valid language code
            RuleFor(dto => dto.DescriptionContent.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(GetTranslation("InvalidLangCode"));


            // Validate that each value in DescriptionContent is not null or empty
            RuleForEach(dto => dto.DescriptionContent.Values)
                .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));
            // Validate that TitleContent is not null and contains at least three entries
            RuleFor(dto => dto.TitleContent)
                .NotNull()
                .WithMessage(GetTranslation("ContentEmpty"))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in TitleContent is a valid language code
            RuleFor(dto => dto.TitleContent.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(GetTranslation("InvalidLangCode"));


            // Validate that each value in TitleContent is not null or empty
            RuleForEach(dto => dto.TitleContent.Values)
                .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
