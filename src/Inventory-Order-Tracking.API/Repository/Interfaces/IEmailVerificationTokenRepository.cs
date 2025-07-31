using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task<EmailVerificationToken> AddToken(EmailVerificationToken token);

        Task<EmailVerificationToken?> GetById(Guid id);

        Task RemoveAsync(EmailVerificationToken token);

        Task SaveChangesAsync();
    }

}