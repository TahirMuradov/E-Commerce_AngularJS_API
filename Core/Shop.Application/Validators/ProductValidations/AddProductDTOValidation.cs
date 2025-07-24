using FluentValidation;
using Shop.Application.DTOs.ProductDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ProductValidations
{
   public class AddProductDTOValidation:AbstractValidator<AddProductDTO>
    {
        private string[] _SupportLanguages { get; set; }
        public AddProductDTOValidation(string LangCode, string[] SupportLanguages)
        {
            _SupportLanguages = SupportLanguages;


            RuleFor(x => x.ProductCode)
    .NotEmpty().WithMessage(_ => GetTranslation("ProductCodeRequired"));

            RuleFor(x => x.Title)
             .NotNull().WithMessage(_ => GetTranslation("ProductNameRequired"))
             .Must(HaveValidLanguages).WithMessage(_ => GetTranslation("InvalidLangCode"))
             .Must(AllValuesNotEmpty).WithMessage(_ => GetTranslation("LangContentTooShort"));

            RuleFor(x => x.CategoryId)
                .Must(x=>x!=Guid.Empty).WithMessage(_ => GetTranslation("CategoryIdRequired"));

            // Validate Description dictionary
            RuleForEach(x => x.Description)
                .Must(keyValuePair => SupportLanguages.Contains(keyValuePair.Key))
                .WithMessage(GetTranslation("InvalidLangCode"));

            // Ensure the number of supported languages matches the number of dictionary entries
            RuleFor(x => x.Title)
                .Must(productName => productName.Count == SupportLanguages.Length)
                .WithMessage(GetTranslation("LangContentTooShort"));

            RuleFor(x => x.Description)
                .Must(description => description.Count == SupportLanguages.Length)
                .WithMessage(GetTranslation("LangContentTooShort"));

            RuleFor(x => x.Sizes)
              .NotNull().WithMessage(_ => GetTranslation("SizesRequired"))
              .Must(AllSizeValuesArePositiveIntegers).WithMessage(_ => GetTranslation("SizesMustBePositiveInt"));

            RuleFor(x => x.ProductImages)
                .NotNull().WithMessage(_ => GetTranslation("PictureİsRequired"))
                .Must(x => x.Count > 0).WithMessage(_ => GetTranslation("PictureİsRequired"));

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(GetTranslation("DisCountChecked"));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(GetTranslation("PriceChecked"));



            // Validate ProductCode
            RuleFor(x => x.ProductCode)
                .NotNull()
               .NotEmpty()
                .WithMessage(GetTranslation("ProductCodeİsRequired"));


        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
        private bool HaveValidLanguages(Dictionary<string, string> dict)
        {
            
            return _SupportLanguages.All(lang => dict.ContainsKey(lang));
        }

        private bool AllValuesNotEmpty(Dictionary<string, string> dict)
        {
            return dict.All(kv => !string.IsNullOrWhiteSpace(kv.Value));
        }

        private bool AllSizeValuesArePositiveIntegers(Dictionary<Guid, int> dict)
        {
            return dict.All(kv => kv.Value > 0);
        }
    }
}
