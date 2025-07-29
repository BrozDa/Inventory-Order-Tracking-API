using FluentValidation;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Validators;

namespace Inventory_Order_Tracking.API.Utils
{
    public class RegisterRequestValidator : AbstractValidator<UserRegistrationDto>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Username)
            .SetValidator(new UsernameValidator());

            RuleFor(x => x.Password)
                .SetValidator(new PasswordValidator());

            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
