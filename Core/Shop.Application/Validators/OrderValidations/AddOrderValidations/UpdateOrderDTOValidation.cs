using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using System.Globalization;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
  public  class UpdateOrderDTOValidation :AbstractValidator<UpdateOrderDTO>
    {
        public UpdateOrderDTOValidation ()
        {
            
                RuleFor(x => x.OrderId)
                    .NotEmpty().WithMessage(_ => GetTranslation("IdIsRequired"))
                    .NotEqual(Guid.Empty).WithMessage(_ => GetTranslation("IdIsRequired"));
                RuleFor(x => x.FullName)
               .NotEmpty().WithMessage(_ => GetTranslation("FullNameIsRequired"))
               .MaximumLength(100).WithMessage(_ => GetTranslation("FullNameMaxLength"));

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty().WithMessage(_ => GetTranslation("PhoneNumberRequired"))
                    .Matches(@"^\+994-\d{2}-\d{3}-\d{2}-\d{2}$")
                    .WithMessage(_ => GetTranslation("PhoneNumberInvalid"));

                RuleFor(x => x.Address)
                    .NotEmpty().WithMessage(_ => GetTranslation("AddressRequired"))
                    .MaximumLength(200).WithMessage(_ => GetTranslation("AddressMaxLength"));

                RuleFor(x => x.Note)
                    .MaximumLength(500).WithMessage(_ => GetTranslation("NoteMaxLength"));


                //RuleFor(x => x.Products)
                //    .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductsRequired", culture));

                //RuleForEach(x => x.Products).SetValidator(new OrderProductDTOValidator(langCode));

                RuleFor(x => x.ShippingMethod)
                    .NotNull().WithMessage(_ => GetTranslation("ShippingMethodRequired"))
                    .SetValidator(new OrderShippingMethodDTOValidator());

                //RuleFor(x => x.PaymentMethod)
                //    .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PaymentMethodRequired", culture))
                //    .SetValidator(new OrderPaymentMethodDTOValidator(langCode));
            }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }

    }
}
