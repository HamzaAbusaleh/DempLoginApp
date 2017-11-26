using FluentValidation;
using LoginAppDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models.Validators
{
    public class PersonValidator : AbstractValidator<PersonModel>
    {
        public PersonValidator()
        {
         
            RuleFor(c => c.Department).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                   .WithMessage("Department is required.");

         

            RuleFor(c => c.Position).Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull()
                 .WithMessage("Position is required.");

            RuleFor(c => c.FirstName).Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull()
                 .WithMessage("FirstName is required.");

            RuleFor(c => c.LastName).Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull()
                 .WithMessage("LastName is required.");

            RuleFor(c => c.Gender).Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull()
                 .WithMessage("Gender is required.");


            RuleFor(c => c.EmployeedFrom).Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
                 .WithMessage("EmployeedFrom is required.");

        }
      
    }
}