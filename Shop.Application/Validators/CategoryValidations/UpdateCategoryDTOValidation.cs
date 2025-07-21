using FluentValidation;
using Shop.Application.DTOs.CategoryDTOs;
using System.Globalization;

namespace Shop.Application.Validators.CategoryValidations
{
   public class UpdateCategoryDTOValidation:AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryDTOValidation(string langCode,string[] SupportLanguages)
        {

            RuleFor(dto => dto.CategoryContent)
               .NotNull()
               .WithMessage(GetTranslation("ContentEmpty"))
               .Must(langContent => langContent != null && langContent.Count == SupportLanguages.Count())
               .WithMessage(GetTranslation("LangContentTooShort"));

            // Validate that each key in LangContent is a valid language code
            RuleFor(dto => dto.CategoryContent.Keys)
       .Must(keys => keys.All(key => (SupportLanguages).Contains(key)))
       .WithMessage(GetTranslation("InvalidLangCode"));


            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.CategoryContent.Values)
                .NotEmpty()
                .WithMessage(GetTranslation("ContentEmpty"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
