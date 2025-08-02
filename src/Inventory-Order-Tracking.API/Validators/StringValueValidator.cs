using FluentValidation;
using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Domain;

namespace Inventory_Order_Tracking.API.Validators
{
    public class StringValueValidator : AbstractValidator<StringWrapper>
    {
        public StringValueValidator()
        {
            RuleFor(data => data.Value)
                .NotNull().WithMessage("Value cannot be empty")
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");
        }
    }
}
