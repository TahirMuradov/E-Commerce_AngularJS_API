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

            var culture = new CultureInfo(LangCode);
            RuleFor(x => x.ProductCode)
    .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductCodeRequired", culture));

            RuleFor(x => x.Title)
             .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductNameRequired", culture))
             .Must(HaveValidLanguages).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture))
             .Must(AllValuesNotEmpty).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleFor(x => x.CategoryId)
                .Must(x=>x!=Guid.Empty).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("CategoryIdRequired", culture));

            // Validate Description dictionary
            RuleForEach(x => x.Description)
                .Must(keyValuePair => SupportLanguages.Contains(keyValuePair.Key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture));

            // Ensure the number of supported languages matches the number of dictionary entries
            RuleFor(x => x.Title)
                .Must(productName => productName.Count == SupportLanguages.Length)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleFor(x => x.Description)
                .Must(description => description.Count == SupportLanguages.Length)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleFor(x => x.Sizes)
              .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("SizesRequired", culture))
              .Must(AllSizeValuesArePositiveIntegers).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("SizesMustBePositiveInt", culture));

            RuleFor(x => x.ProductImages)
                .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PictureİsRequired", culture))
                .Must(x => x.Count > 0).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PictureİsRequired", culture));

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("DisCountChecked", culture));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("PriceChecked", culture));



            // Validate ProductCode
            RuleFor(x => x.ProductCode)
                .NotNull()
               .NotEmpty()
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("ProductCodeİsRequired", new CultureInfo(LangCode)));


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
