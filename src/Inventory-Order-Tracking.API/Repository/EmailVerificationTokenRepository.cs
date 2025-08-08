using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    /// <summary>
    /// Provides data access and persistence operations for <see cref="EmailVerificationToken"/> entities.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IEmailVerificationTokenRepository"/> interface to interact with the database
    /// using Entity Framework Core.
    /// </remarks>
    public class EmailVerificationTokenRepository(InventoryManagementContext context) : IEmailVerificationTokenRepository
    {
        /// <inheritdoc/>
        public async Task<EmailVerificationToken> AddTokenAsync(EmailVerificationToken token)
        {
            await context.EmailVerificationTokens.AddAsync(token);
            await context.SaveChangesAsync();
            return token;
        }

        /// <inheritdoc/>
        public async Task<EmailVerificationToken?> GetByIdAsync(Guid id)
        {
            return await context.EmailVerificationTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(EmailVerificationToken token)
        {
            context.EmailVerificationTokens.Remove(token);
            await SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}