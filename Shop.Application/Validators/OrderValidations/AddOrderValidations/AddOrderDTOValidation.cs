using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;
using System.Globalization;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class AddOrderDTOValidation:AbstractValidator<AddOrderDTO>
    {
        public AddOrderDTOValidation(string langCode)
        {
       
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage(_ => GetTranslation("UserIDRequired"))
                .NotEqual(Guid.Empty).WithMessage(_ => GetTranslation("UserIDInvalid"));
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


            RuleFor(x => x.Products)
                .NotEmpty().WithMessage(_ => GetTranslation("ProductsRequired"));

            RuleForEach(x => x.Products).SetValidator(new OrderProductDTOValidator());

            RuleFor(x => x.ShippingMethod)
                .NotNull().WithMessage(_ =>     GetTranslation("ShippingMethodRequired"))
                .SetValidator(new OrderShippingMethodDTOValidator());

            RuleFor(x => x.PaymentMethod)
                .NotNull().WithMessage(_ => GetTranslation("PaymentMethodRequired"))
                .SetValidator(new OrderPaymentMethodDTOValidator());
        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
