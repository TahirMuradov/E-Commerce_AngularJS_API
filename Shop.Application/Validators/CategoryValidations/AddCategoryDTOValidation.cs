using FluentValidation;
using Shop.Application.DTOs.CategoryDTOs;
using System.Globalization;
namespace Shop.Application.Validators.CategoryValidations
{
   public class AddCategoryDTOValidation:AbstractValidator<AddCategoryDTO>

    {
        public AddCategoryDTOValidation(string langCode, string[] SupportLanguages)
        {
           
            RuleFor(dto => dto.CategoryContent)
               .NotNull()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", new CultureInfo(langCode)))
               .Must(langContent => langContent != null && langContent.Count == SupportLanguages.Count())
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", new CultureInfo(langCode)));

            // Validate that each key in LangContent is a valid language code
            RuleFor(dto => dto.CategoryContent.Keys)
       .Must(keys => keys.All(key => (SupportLanguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", new CultureInfo(langCode)));


            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.CategoryContent.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", new CultureInfo(langCode)));

        }
    }
}
