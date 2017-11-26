using FluentValidation;
using LoginAppDemo.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginAppDemo.Models
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleFor(c => c.UserName).Cascade(CascadeMode.StopOnFirstFailure)
               .NotNull()
                   .WithMessage("User name is required.");
               

            RuleFor(c => c.Email).Cascade(CascadeMode.StopOnFirstFailure)
              .NotNull()
                  .WithMessage("Email is required.")
              .EmailAddress()
                  .WithMessage("Email must be a valid email address.");
            RuleFor(c => c.CompanyName).Cascade(CascadeMode.StopOnFirstFailure)
             .NotNull()
                 .WithMessage("Company name is required.");

            RuleFor(c => c.Password).Cascade(CascadeMode.StopOnFirstFailure)
               
                   .Must(BeMinimumEightCharacters)
                       .When(p => !string.IsNullOrWhiteSpace(p.Password))
                       .WithMessage("Password must be a minimum of 8 characters.")
                   .Must(ContainAtLeastOneUppercaseCharacter)
                       .When(p => !string.IsNullOrWhiteSpace(p.Password))
                       .WithMessage("Password must contain at least 1 upper case character.")
                   .Must(ContainAtLeastOneNumber)
                       .When(p => !string.IsNullOrWhiteSpace(p.Password))
                       .WithMessage("Password must contain at least one number.")
                   .NotNull()
                   .WithMessage("Password is required."); ;

            RuleFor(c => c.PasswordConfirmation).Cascade(CascadeMode.StopOnFirstFailure)
                
                .Must((password, passwordConfirmation) => Match(passwordConfirmation, password.Password))
                .When(c => !string.IsNullOrWhiteSpace(c.Password))
                    .WithMessage("Password and password confirmation do not match.")
                .NotNull()
                   .WithMessage("Password confirmation is required.");

        }
         
        private static bool BeMinimumEightCharacters(string password)
        {
            return password.Length >= 8;
        }

        private static bool ContainAtLeastOneUppercaseCharacter(string password)
        {
            return password.Any(char.IsUpper);
        }

        private static bool ContainAtLeastOneNumber(string password)
        {
            return password.Any(char.IsDigit);
        }
        private static bool Match(string passwordConfirmation, string password)
        {
            return passwordConfirmation == password;
        }
    }
}