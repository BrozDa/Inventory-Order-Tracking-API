using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<EmailVerificationServiceResult> SendVerificationEmail(User user);

        Task<EmailVerificationServiceResult> VerifyEmail(Guid tokenId);
    }
}