using FluentValidation;
using Shop.Application.DTOs.CategoryDTOs;
using System.Globalization;

namespace Shop.Application.Validators.CategoryValidations
{
   public class UpdateCategoryDTOValidation:AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryDTOValidation(string langCode,string[] SupportLanguages)
        {
            var culture = new CultureInfo(langCode);
            RuleFor(dto => dto.CategoryContent)
               .NotNull()
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty",culture))
               .Must(langContent => langContent != null && langContent.Count == SupportLanguages.Count())
               .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            // Validate that each key in LangContent is a valid language code
            RuleFor(dto => dto.CategoryContent.Keys)
       .Must(keys => keys.All(key => (SupportLanguages).Contains(key)))
       .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture));


            // Validate that each value in LangContent is not null or empty
            RuleForEach(dto => dto.CategoryContent.Values)
                .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ContentEmpty", culture));

        }
    }
}
