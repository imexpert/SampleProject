using FluentValidation;
using SampleProject.Business.Handlers.Authorizations.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Business.Handlers.Authorizations.ValidationRules
{
    public class LoginUserValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserValidator()
        {
            RuleFor(m => m.LoginModel.Password).NotNull().WithMessage("Password cannot be empty");
            RuleFor(m => m.LoginModel.Username).NotNull().WithMessage("Username cannot be empty");
        }
    }
}
