using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations related to email verification.
    /// </summary>
    public interface IEmailVerificationService
    {
        /// <summary>
        /// Sends a verification to users email address
        /// </summary>
        /// <param name="user">An <see cref="User"/> for whom the email will be sent</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing nothing on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<object>> SendVerificationEmailAsync(User user);

        /// <summary>
        /// Verifies users email address
        /// </summary>
        /// <param name="tokenId">An <see cref="User"/> Id for whom the email will be verified</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing nothing on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<object>> VerifyEmailAsync(Guid tokenId);
    }
}