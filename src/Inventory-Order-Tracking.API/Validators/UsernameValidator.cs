using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
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