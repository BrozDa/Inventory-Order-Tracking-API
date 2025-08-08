using FluentValidation;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Validators;

namespace Inventory_Order_Tracking.API.Utils
{
    /// <summary>
    /// Validates the all fields in <see cref="UserRegistrationDto"/> when registering new user.
    /// Uses specific validators for Username, Password, and Email to ensure user registration input is valid.
    /// </summary>
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