using FluentValidation;
using Shop.Application.DTOs.WebUI.TopCategoryAreaDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.TopCategoryAreaDTOValidations
{
  public  class UpdateTopCategoryAreaDTOValidation :AbstractValidator<UpdateTopCategoryAreaDTO>
    {
        public UpdateTopCategoryAreaDTOValidation( string[] SupportedLaunguages)
        {
            
            RuleFor(x => x.Id)
            .NotNull()
             .WithMessage(GetTranslation("IdRequired"))
            .NotEmpty()
             .WithMessage(GetTranslation("IdRequired"))
            .NotEqual(Guid.Empty)
             .WithMessage(GetTranslation("IdInvalid"));
            // Validate that DescriptionContent is not null and contains at least three entries
            RuleFor(dto => dto.Description)
            .NotNull()
                .WithMessage(GetTranslation("ContentEmpty"))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in DescriptionContent is a valid language code
            RuleFor(dto => dto.Description.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(GetTranslation("InvalidLangCode"));


            // Validate that each value in DescriptionContent is not null or empty
            RuleForEach(dto => dto.Description.Values)
            .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));
            // Validate that TitleContent is not null and contains at least three entries
            RuleFor(dto => dto.Title)
            .NotNull()
                .WithMessage(GetTranslation("ContentEmpty"))
                .Must(langContent => langContent != null && langContent.Count > 3)
                .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in TitleContent is a valid language code
            RuleFor(dto => dto.Title.Keys)
       .Must(keys => keys.All(key => (SupportedLaunguages).Contains(key)))
       .WithMessage(GetTranslation("InvalidLangCode"));


            // Validate that each value in TitleContent is not null or empty
            RuleForEach(dto => dto.Title.Values)
            .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));

            //// Validate Pictures
            RuleFor(x => x)
                .Must(dto => dto.CurrentPictureUrl is null && dto.NewImage is null)
                .WithMessage(GetTranslation("PictureİsRequired"));
            RuleFor(x => x)
            .Must(dto => !((dto.CategoryId == null || dto.CategoryId == Guid.Empty) &&
            (dto.SubCategoryId == null || dto.SubCategoryId == Guid.Empty)))
                .WithMessage(GetTranslation("SubCategoryAndCategoryIdIsrequired"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }

    }
}
