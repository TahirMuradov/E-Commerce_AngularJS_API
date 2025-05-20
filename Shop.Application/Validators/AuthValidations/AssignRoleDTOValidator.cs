using FluentValidation;
using Shop.Application.DTOs.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Validators.AuthValidations
{
   public class AssignRoleDTOValidator:AbstractValidator<AssignRoleDTO>
    {
        public AssignRoleDTOValidator(string LangCode)
        {
            RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdRequired", new CultureInfo(LangCode)))
            .NotEqual(Guid.Empty).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("UserIdInvalid", new CultureInfo(LangCode)));

        
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdRequired", new CultureInfo(LangCode)))
                .Must(x => x != null && x != default).WithMessage(ValidatorOptions.Global.LanguageManager.GetString("RoleIdInvalid", new CultureInfo(LangCode)));

        }
    }
}
