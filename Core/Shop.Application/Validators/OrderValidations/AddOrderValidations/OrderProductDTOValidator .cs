using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using System.Globalization;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class OrderProductDTOValidator:AbstractValidator<OrderProductDTO>
    {
        public OrderProductDTOValidator()
        {
  

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_ => GetTranslation("ProductIdIsRequired"));

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage(_ => GetTranslation("ProductCodeIsRequired"));

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage(_ => GetTranslation("ProductNameIsRequired"));

            RuleFor(x => x.size)
                .NotEmpty().WithMessage(_ => GetTranslation("SizeIsRequired"));

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(_ => GetTranslation("QuantityMustBeGreaterThanZero"));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(_ => GetTranslation("PriceMustBeNonNegative"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
