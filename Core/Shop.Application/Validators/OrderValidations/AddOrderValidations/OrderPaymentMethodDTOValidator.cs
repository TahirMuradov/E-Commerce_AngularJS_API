using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class OrderPaymentMethodDTOValidator : AbstractValidator<OrderPaymentMethodDTO>
    {
        public OrderPaymentMethodDTOValidator()
        {
          
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(_ => GetTranslation("PaymentIdIsRequired"));

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage(_ => GetTranslation("PaymentContentIsRequired"));
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new System.Globalization.CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
