using FluentValidation;
using LoginAppDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models
{
    public class UserLoginValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginValidator()
        {
            RuleFor(c => c.UserName).Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull()
                   .WithMessage("User name is required.");
                

            RuleFor(c => c.Password).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                   .WithMessage("Password is required.");
        }
       
    }
}