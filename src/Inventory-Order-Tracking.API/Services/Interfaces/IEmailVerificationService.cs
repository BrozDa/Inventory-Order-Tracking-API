using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IEmailVerificationService
    {
        Task<ServiceResult<object>> SendVerificationEmailAsync(User user);

        Task<ServiceResult<object>> VerifyEmailAsync(Guid tokenId);
    }
}