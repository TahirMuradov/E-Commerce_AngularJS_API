using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class OrderProductDTOValidator:AbstractValidator<OrderProductDTO>
    {
        public OrderProductDTOValidator(string langCode)
        {
            var culture= new System.Globalization.CultureInfo(langCode);

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductIdIsRequired", culture));

            RuleFor(x => x.ProductCode)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductCodeIsRequired", culture));

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductNameIsRequired", culture));

            RuleFor(x => x.size)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("SizeIsRequired", culture));

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("QuantityMustBeGreaterThanZero", culture));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PriceMustBeNonNegative", culture));
        }
    }
}
