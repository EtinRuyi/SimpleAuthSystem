using FluentValidation;
using SimpleAuthSystem.Application.DTOs.RequestDTOs;

namespace SimpleAuthSystem.Application.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[!@#$%^&*?]").WithMessage("Password must contain at least one special character")
            .WithName("Password");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match")
                .WithName("Confirm Password");
        }
    }
}
