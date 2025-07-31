using FluentEmail.Core;
using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Interfaces;

namespace Inventory_Order_Tracking.API.Services
{
    public class EmailValidatingService(IFluentEmail emailService, EmailSettings emailSettings) : IEmailValidatingService
    {
        public async Task SendVerificationEmail(User user)
        {
            await emailService
                .To(user.Email)
                .Subject("Verification - Inventory Management dashboard")
                .Body("Click on link to verify your email <a>Link</a>")
                .SendAsync();
        }
    }
}
