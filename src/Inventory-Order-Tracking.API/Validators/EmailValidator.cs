using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(email => email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .Matches(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Entered email address invalid");
        }
    }
}