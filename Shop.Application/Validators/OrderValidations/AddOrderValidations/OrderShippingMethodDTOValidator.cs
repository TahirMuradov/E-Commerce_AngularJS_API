using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
    public class OrderShippingMethodDTOValidator : AbstractValidator<OrderShippingMethodDTO>
    {
        public OrderShippingMethodDTOValidator(string langCode)
        {
            var culture = new System.Globalization.CultureInfo(langCode);
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ShippingIdIsRequired", culture));

            RuleFor(x => x.ShippingContent)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ShippingContentIsRequired", culture));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ShippingPriceMustBeNonNegative", culture));
        }
    }
}
