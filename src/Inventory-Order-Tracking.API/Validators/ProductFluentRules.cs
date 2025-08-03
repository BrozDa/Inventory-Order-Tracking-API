using FluentValidation;

namespace Inventory_Order_Tracking.API.Validators
{
    public static class ProductFluentRules
    {
        public static IRuleBuilder<T, string?> MustBeValidStringValue<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");
        }

        public static IRuleBuilder<T, int?> MustBePositiveOrNullInt<T>(
            this IRuleBuilder<T, int?> ruleBuilder) {

            return ruleBuilder
                .Must(val => val is not null ||  val > 0)
                .WithMessage("Stock must be  positive number");
        }
        public static IRuleBuilder<T, decimal> MustBePositiveDecimal<T>(
            this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder
                .Must(val => val > 0)
                .WithMessage("Price must be  positive number");
        }
    }
}
