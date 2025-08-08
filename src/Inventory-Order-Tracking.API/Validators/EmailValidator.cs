using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
    /// <summary>
    /// Validates the email in registration request is not empty and in valid email format.
    /// </summary>
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(email => email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .Matches(@"^$|^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Entered email address invalid");
                //Intentionally used simple email validation due to nature of the project
        }
    }
}