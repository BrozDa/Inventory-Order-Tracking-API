using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
    /// <summary>
    /// Validates the password in registration request is not empty and in valid format.
    /// Valid PW - One upperCase, oneLowerCase, one number, 8+ characters.
    /// </summary>
    public class PasswordValidator : AbstractValidator<string>
    {
        public PasswordValidator()
        {

            RuleFor(password => password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password have to be at least 8 characters long")
                .Matches(@"[A-Z]").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[\d]").WithMessage("Your password must contain at least one number.");
        }
    }
}