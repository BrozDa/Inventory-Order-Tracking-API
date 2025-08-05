using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task<EmailVerificationToken> AddTokenAsync(EmailVerificationToken token);

        Task<EmailVerificationToken?> GetByIdAsync(Guid id);

        Task RemoveAsync(EmailVerificationToken token);

        Task SaveChangesAsync();
    }
}