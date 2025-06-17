using FluentValidation;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.TopCategoryAreaDTOValidations
{
  public  class UpdateTopCategoryAreaDTOValidation :AbstractValidator<UpdateTopCategoryAreaDTO>
    {
        public UpdateTopCategoryAreaDTOValidation(string langCode, string[] SupportedLaunguages)
        {
            CultureInfo cultureInfo = new CultureInfo(langCode);
            RuleFor(x => x.Id)
            .NotNull()
             .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdRequired",cultureInfo))
            .NotEmpty()
             .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdRequired",cultureInfo))
            .NotEqual(Guid.Empty)
             .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("IdInvalid",cultureInfo));
            // Validate that DescriptionContent is not null and contains at least three entries
            RuleFor(dto => dto.Description)
            .NotNull()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",cultureInfo))
                .Must(langContent => langContent != null && langContent.Count > 3)
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
                .Must(langContent => langContent != null && langContent.Count > 3)
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
            RuleFor(x => x)
                .Must(dto => dto.CurrentPictureUrl is null && dto.NewImage is null)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PictureİsRequired",cultureInfo));
            RuleFor(x => x)
            .Must(dto => !((dto.CategoryId == null || dto.CategoryId == Guid.Empty) &&
            (dto.SubCategoryId == null || dto.SubCategoryId == Guid.Empty)))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("SubCategoryAndCategoryIdIsrequired",cultureInfo));

        }

    }
}
