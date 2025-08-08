using FluentValidation;
using Inventory_Order_Tracking.API.Configuration;

namespace Inventory_Order_Tracking.API.Validators
{
    /// <summary>
    /// Validates the <see cref="EmailSettings"/> configuration from appsettings.
    /// Ensures all required SMTP fields are present and valid.
    /// </summary>
    public class EmailSettingsValidator : AbstractValidator<EmailSettings>
    {
        public EmailSettingsValidator()
        {
            RuleFor(x => x.SenderEmail)
                .NotEmpty()
                .NotNull()
                .WithMessage("Failed to load SenderEmail from appconfig");

            RuleFor(x => x.Sender)
                .NotEmpty()
                .NotNull()
                .WithMessage("Failed to load Sender from appconfig");

            RuleFor(x => x.Host)
               .NotEmpty()
               .NotNull()
               .WithMessage("Failed to load Host from appconfig");

            RuleFor(x => x.Port)
               .Must(x => x > 0)
               .WithMessage("Failed to load Port from appconfig");
        }
    }
}