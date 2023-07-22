using FluentValidation;
using SampleProject.Business.Handlers.Authorizations.Commands;
using SampleProject.Business.Handlers.Authorizations.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Business.Handlers.Authorizations.ValidationRules
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(m => m.Model.Firstname).NotNull().WithMessage("Firstname cannot be empty");
            RuleFor(m => m.Model.Lastname).NotNull().WithMessage("Lastname cannot be empty");
            RuleFor(m => m.Model.Username).NotNull().WithMessage("Username cannot be empty");
            RuleFor(m => m.Model.Password).NotNull().WithMessage("Password cannot be empty").MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
