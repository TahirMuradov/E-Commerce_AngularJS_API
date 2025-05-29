using FluentValidation;
using Shop.Application.DTOs.OrderPdfGeneratorDTOs;

namespace Shop.Application.Validators.OrderValidations.AddOrderValidations
{
   public class AddOrderDTOValidation:AbstractValidator<AddOrderDTO>
    {
        public AddOrderDTOValidation(string langCode)
        {
            var culture = new System.Globalization.CultureInfo(langCode);
            RuleFor(x => x.UserID)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("UserIDRequired",culture))
                .NotEqual(Guid.Empty).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("UserIDInvalid", culture));
            RuleFor(x => x.FullName)
           .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("FullNameIsRequired", culture))
           .MaximumLength(100).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("FullNameMaxLength", culture));

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberRequired", culture))
                .Matches(@"^\+994-\d{2}-\d{3}-\d{2}-\d{2}$")
                .WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PhoneNumberInvalid", culture));

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("AddressRequired", culture))
                .MaximumLength(200).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("AddressMaxLength", culture));

            RuleFor(x => x.Note)
                .MaximumLength(500).WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("NoteMaxLength", culture));


            RuleFor(x => x.Products)
                .NotEmpty().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ProductsRequired", culture));

            RuleForEach(x => x.Products).SetValidator(new OrderProductDTOValidator(langCode));

            RuleFor(x => x.ShippingMethod)
                .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("ShippingMethodRequired", culture))
                .SetValidator(new OrderShippingMethodDTOValidator(langCode));

            RuleFor(x => x.PaymentMethod)
                .NotNull().WithMessage(_ => ValidatorOptions.Global.LanguageManager.GetString("PaymentMethodRequired", culture))
                .SetValidator(new OrderPaymentMethodDTOValidator(langCode));
        }
    }
}
