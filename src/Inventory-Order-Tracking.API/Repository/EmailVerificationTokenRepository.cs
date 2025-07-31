using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    public class EmailVerificationTokenRepository(InventoryManagementContext context) : IEmailVerificationTokenRepository
    {

        public async Task<EmailVerificationToken> AddToken(EmailVerificationToken token)
        {
            await context.EmailVerificationTokens.AddAsync(token);
            await context.SaveChangesAsync();
            return token;
        }
        public async Task<EmailVerificationToken?> GetById(Guid id)
        {
            return await context.EmailVerificationTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t =>  t.Id == id);
        }
        public async Task RemoveAsync(EmailVerificationToken token)
        {
            context.EmailVerificationTokens.Remove(token);
            await SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}
