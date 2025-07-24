using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System.Globalization;

namespace Shop.Application.Validators.AuthValidations
{
   public class AssignRoleDTOValidator:AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleDTOValidator()
        {
           
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(GetTranslation("UserIdRequired"))
            .NotEqual(Guid.Empty).WithMessage(GetTranslation("UserIdInvalid"));

        
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(GetTranslation("RoleIdRequired"))
                .Must(x => x != null && x != default).WithMessage(GetTranslation("RoleIdInvalid"));

        }
        private string GetTranslation(string key)
        {
            return ValidatorOptions.Global.LanguageManager.GetString(key, new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name));
        }
    }
}
