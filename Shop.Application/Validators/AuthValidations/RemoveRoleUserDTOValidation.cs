using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
    public class RemoveRoleUserDTOValidation:AbstractValidator<RemoveRoleUserDTO>
    {
        public RemoveRoleUserDTOValidation(string LangCode)
        {
            var culture = new CultureInfo(LangCode);

            // UserId validation: must not be empty or default GUID value
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage(GetTranslation("UserIdRequired"))
                .NotEqual(Guid.Empty).WithMessage(GetTranslation("UserIdInvalid"));

            // RoleId validation: must not be null or empty, and at least one role ID is required
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(GetTranslation("RoleIdRequired"))
                .Must(x => x != null && x.Length > 0).WithMessage(GetTranslation("RoleIdInvalid"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
