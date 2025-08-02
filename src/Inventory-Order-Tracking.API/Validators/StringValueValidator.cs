using FluentValidation;
using Inventory_Order_Tracking.API.Configuration;

namespace Inventory_Order_Tracking.API.Validators
{
    public class StringValueValidator : AbstractValidator<string>
    {
        public StringValueValidator()
        {
            RuleFor(stringValue => stringValue)
                .NotEmpty().NotNull().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");
        }
    }
}
