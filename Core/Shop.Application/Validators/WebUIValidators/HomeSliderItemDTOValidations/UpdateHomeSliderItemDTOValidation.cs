using FluentValidation;
using Shop.Application.DTOs.WebUI.HomeSliderItemDTOs;
using System.Globalization;

namespace Shop.Application.Validators.WebUIValidators.HomeSliderItemDTOValidations
{
   public class UpdateHomeSliderItemDTOValidation:AbstractValidator<UpdateHomeSliderItemDTO>
    {
        public UpdateHomeSliderItemDTOValidation( string[] SupportedLaunguages)
        {
      

            RuleFor(x => x.Id)
              .NotNull()
              .WithMessage(GetTranslation("IdRequired"))
              .NotEmpty()
              .WithMessage(GetTranslation("IdRequired"))
              .NotEqual(Guid.Empty)
              .WithMessage(GetTranslation("IdInvalid"));


            //// Validate CurrentPictureUrls and NewPictures
            //RuleFor(x => x)
            //    .Must(x => !(x.CurrentPictureUrls == null) || !(x.NewImage == null || x.NewImage == default))
            //    .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PictureUrlsOrNewPicturesRequired",cultureInfo));

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



        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
