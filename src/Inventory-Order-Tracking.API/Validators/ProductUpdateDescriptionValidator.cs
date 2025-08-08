using FluentValidation;
using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Validators
{

    /// <summary>
    /// Validates new description within <see cref="ProductUpdateDescriptionDto"/> when updating.
    /// </summary>
    public class ProductUpdateDescriptionValidator : AbstractValidator<ProductUpdateDescriptionDto>
    {
        public ProductUpdateDescriptionValidator()
        {
            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Value cannot be empty")
               .Length(1, 50).WithMessage("Value must be between 1 and 50 characters")
               .Matches(@"^[a-zA-Z0-9\- ]+$").WithMessage("Value must contain only alphanumeric characters, space or dash");
        }
    }
}