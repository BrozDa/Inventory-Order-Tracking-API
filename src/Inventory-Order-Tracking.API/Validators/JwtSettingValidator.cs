using FluentValidation;
using Inventory_Order_Tracking.API.Configuration;

namespace Inventory_Order_Tracking.API.Validators
{
    public class JwtSettingValidator : AbstractValidator<JwtSettings>
    {
        public JwtSettingValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .NotNull()
                .WithMessage("JWT key is missing or invalid.");

            RuleFor(x => x.Issuer)
                .NotEmpty()
                .NotNull()
                .WithMessage("JWT issuer is missing or invalid.");

            RuleFor(x => x.Audience)
                .NotEmpty()
                .NotNull()
                .WithMessage("JWT audience is missing or invalid.");

            RuleFor(x => x.TokenExpirationDays)
                .NotEmpty()
                .NotNull()
                .Must(x => x > 0)
                .WithMessage("JWT expiration is missing or invalid.");

            RuleFor(x => x.RefreshTokenExpirationDays)
                .NotEmpty()
                .NotNull()
                .Must(x => x > 0)
                .WithMessage("JWT refresh token expiration is missing or invalid.");
        }
    }
}