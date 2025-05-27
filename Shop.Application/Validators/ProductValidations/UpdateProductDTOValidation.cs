using FluentValidation;
using Shop.Application.DTOs.ProductDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ProductValidations
{
    public class UpdateProductDTOValidation : AbstractValidator<UpdateProductDTO>
    {
        private string[] _SupportLanguages { get; set; }

        public UpdateProductDTOValidation(string langCode, string[] SupportLanguages)
        {
            _SupportLanguages = SupportLanguages;
            var culture = new CultureInfo(langCode);

            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(id => id != Guid.Empty)
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("IdIsRequired", culture));

            RuleFor(x => x.ProductCode)
                .NotEmpty()
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductCodeRequired", culture));

            RuleFor(x => x.Title)
                .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductNameRequired", culture))
                .Must(HaveValidLanguages).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture))
                .Must(AllValuesNotEmpty).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleForEach(x => x.Description)
                .Must(kv => SupportLanguages.Contains(kv.Key))
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("InvalidLangCode", culture));

            RuleFor(x => x.Title)
                .Must(title => title.Count == SupportLanguages.Length)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleFor(x => x.Description)
                .Must(description => description.Count == SupportLanguages.Length)
                .WithMessage(ValidatorOptions.Global.LanguageManager.GetString("LangContentTooShort", culture));

            RuleFor(x => x.Sizes)
                .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("SizesRequired", culture))
                .Must(AllSizeValuesArePositiveIntegers)
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("SizesMustBePositiveInt", culture));

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("DisCountChecked", culture));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PriceChecked", culture));
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
