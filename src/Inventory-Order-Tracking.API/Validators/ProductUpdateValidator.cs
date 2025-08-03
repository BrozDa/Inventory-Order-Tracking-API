using FluentValidation;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Validators
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash")
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Value cannot be empty")
                .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
                .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash")
                .When(x => x.Description is not null);

            RuleFor(x => x.Price)
                .Must(val => val > 0)
                .WithMessage("Price must be  positive number")
                .When(x => x.Price is not null);

            RuleFor(x => x.StockQuantity)
                .Must(val => val > 0)
                .WithMessage("Stock must be  positive number")
                .When(x => x.StockQuantity is not null);

        }
    }
}
