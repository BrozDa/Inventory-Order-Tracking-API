using FluentValidation;
using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Validators
{
    /// <summary>
    /// Validates the all fields in <see cref="ProductAddDto"/> when adding a new product.
    /// </summary>
    public class ProductAddValidator : AbstractValidator<ProductAddDto>
    {
        public ProductAddValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");

            RuleFor(x => x.Price)
                .Must(val => val > 0)
                .WithMessage("Price must be  positive number");

            RuleFor(x => x.StockQuantity)
                .Must(val => val is null || val > 0)
                .WithMessage("Stock must be  positive number");
        }
    }
}