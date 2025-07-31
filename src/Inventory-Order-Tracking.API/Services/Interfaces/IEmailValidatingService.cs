using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IEmailValidatingService
    {
        Task SendVerificationEmail(User user);
    }
}
