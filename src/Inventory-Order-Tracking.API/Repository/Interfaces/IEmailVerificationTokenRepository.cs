using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    /// <summary>
    /// Defines a contract for accessing and managing verification tokens in data storage.
    /// </summary>
    public interface IEmailVerificationTokenRepository
    {
        /// <summary>
        /// Adds new token to the data storage
        /// </summary>
        /// <param name="token">A <see cref="EmailVerificationToken"/> to be added </param>
        /// <returns>Newly added instance of <see cref="EmailVerificationToken"/></returns>
        Task<EmailVerificationToken> AddTokenAsync(EmailVerificationToken token);

        /// <summary>
        /// Retrieves single <see cref="EmailVerificationToken"/> from data storage based on provided id.
        /// </summary>
        /// <param name="id">An Id of a <see cref="EmailVerificationToken"/> to be retrieved</param>
        /// <returns>A retrieved <see cref="EmailVerificationToken"/> if its found, null otherwise</returns>
        Task<EmailVerificationToken?> GetByIdAsync(Guid id);

        /// <summary>
        /// Removes single <see cref="EmailVerificationToken"/> from data storage based on provided id.
        /// </summary>
        /// <param name="id">An Id of a <see cref="EmailVerificationToken"/> to be retrieved</param>
        Task RemoveAsync(EmailVerificationToken token);

        /// <summary>
        /// Persists all pending changes made to entities in data storage.
        /// </summary>
        Task SaveChangesAsync();
    }
}