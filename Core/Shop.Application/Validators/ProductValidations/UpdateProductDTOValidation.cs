using FluentValidation;
using Shop.Application.DTOs.ProductDTOs;
using System.Globalization;

namespace Shop.Application.Validators.ProductValidations
{
    public class UpdateProductDTOValidation : AbstractValidator<UpdateProductDTO>
    {
        private string[] _SupportLanguages { get; set; }

        public UpdateProductDTOValidation( string[] SupportLanguages)
        {
            _SupportLanguages = SupportLanguages;
            

            RuleFor(x => x.Id)
                         .Must(id => id != Guid.Empty)
                .WithMessage(_ => GetTranslation("IdIsRequired"));
            RuleFor(x => x.CategoryId)
           .Must(id => id != Guid.Empty)
           .WithMessage(_ => GetTranslation("CategoryIdInvalid"));

            RuleFor(x => x.ProductCode)
                .NotEmpty()
                .WithMessage(_ => GetTranslation("ProductCodeRequired"));

            RuleFor(x => x.Title)
                .NotNull().WithMessage(_ => GetTranslation("ProductNameRequired"))
                .Must(HaveValidLanguages).WithMessage(_ => GetTranslation("InvalidLangCode"))
                .Must(AllValuesNotEmpty).WithMessage(_ => GetTranslation("LangContentTooShort"));

            RuleForEach(x => x.Description)
                .Must(kv => SupportLanguages.Contains(kv.Key))
                .WithMessage(GetTranslation("InvalidLangCode"));

            RuleFor(x => x.Title)
                .Must(title => title.Count == SupportLanguages.Length)
                .WithMessage(GetTranslation("LangContentTooShort"));

            RuleFor(x => x.Description)
                .Must(description => description.Count == SupportLanguages.Length)
                .WithMessage(GetTranslation("LangContentTooShort"));

            RuleFor(x => x.Sizes)
                .NotNull().WithMessage(_ => GetTranslation("SizesRequired"))
                .Must(AllSizeValuesArePositiveIntegers)
                .WithMessage(_ => GetTranslation("SizesMustBePositiveInt"));

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0)
                .WithMessage(_ => GetTranslation("DisCountChecked"));

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage(_ => GetTranslation("PriceChecked"));
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
