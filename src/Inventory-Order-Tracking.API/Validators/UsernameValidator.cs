using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
    /// <summary>
    /// Validates the username in registration request is not empty and in valid format.
    /// Valid Username - between 5 and 16 characters, only alphanumeric characters.
    /// </summary>
    public class UsernameValidator : AbstractValidator<string>
    {
        public UsernameValidator()
        {
            RuleFor(username => username)
                .NotEmpty().WithMessage("Username cannot be empty")
                .MinimumLength(5).WithMessage("Username have to have minimum of 5 characters")
                .MaximumLength(16).WithMessage("Username have to have maximum of 16 characters")
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage("Your password must only alphanumeric characters.");
        }
    }
}