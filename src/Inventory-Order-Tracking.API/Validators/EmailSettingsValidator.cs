using FluentValidation;
using Inventory_Order_Tracking.API.Configuration;

namespace Inventory_Order_Tracking.API.Validators
{
    public class EmailSettingsValidator :AbstractValidator<EmailSettings>
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;

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
               .NotEmpty()
               .NotNull()
               .Must(x => x > 0)
               .WithMessage("Failed to load Port from appconfig");
        }
    }
}
