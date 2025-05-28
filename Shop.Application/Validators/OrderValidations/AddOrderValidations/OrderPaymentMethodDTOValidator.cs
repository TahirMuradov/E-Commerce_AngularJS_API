using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class OrderPaymentMethodDTOValidator : AbstractValidator<OrderPaymentMethodDTO>
    {
        public OrderPaymentMethodDTOValidator(string langCode)
        {
            var culture = new System.Globalization.CultureInfo(langCode);
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PaymentIdIsRequired", culture));

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PaymentContentIsRequired", culture));
        }
    }
}
