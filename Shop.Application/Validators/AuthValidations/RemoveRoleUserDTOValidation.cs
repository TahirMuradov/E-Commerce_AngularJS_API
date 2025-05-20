using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
    public class RemoveRoleUserDTOValidation:AbstractValidator<RemoveRoleUserDTO>
    {
        public RemoveRoleUserDTOValidation(string LangCode)
        {

            // UserId validation: must not be empty or default GUID value
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdRequired", new CultureInfo(LangCode)))
                .NotEqual(Guid.Empty).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdInvalid", new CultureInfo(LangCode)));

            // RoleId validation: must not be null or empty, and at least one role ID is required
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdRequired", new CultureInfo(LangCode)))
                .Must(x => x != null && x.Length > 0).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdInvalid", new CultureInfo(LangCode)));

        }
    }
}
