using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
   public class AssignRoleDTOValidator:AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleDTOValidator(string LangCode)
        {
            var culture = new CultureInfo(LangCode);
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdRequired", culture))
            .NotEqual(Guid.Empty).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdInvalid", culture));

        
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdRequired", culture))
                .Must(x => x != null && x != default).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdInvalid", culture));

        }
    }
}
