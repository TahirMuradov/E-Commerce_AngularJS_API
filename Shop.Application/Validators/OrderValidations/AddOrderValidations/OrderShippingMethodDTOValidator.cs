using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using System.Globalization;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
    public class OrderShippingMethodDTOValidator : AbstractValidator<OrderShippingMethodDTO>
    {
        public OrderShippingMethodDTOValidator()
        {
 
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_ => GetTranslation("ShippingIdIsRequired"));

            RuleFor(x => x.ShippingContent)
                .NotEmpty().WithMessage(_ => GetTranslation("ShippingContentIsRequired"));

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage(_ => GetTranslation("ShippingPriceMustBeNonNegative"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
