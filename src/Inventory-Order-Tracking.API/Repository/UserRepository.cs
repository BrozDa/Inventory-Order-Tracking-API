using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    /// <summary>
    /// Provides data access and persistence operations for <see cref="User"/> entities.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IUserRepository"/> interface to interact with the database
    /// using Entity Framework Core.
    /// </remarks>
    public class UserRepository(InventoryManagementContext context) : IUserRepository
    {
        /// <inheritdoc/>
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        /// <inheritdoc/>
        public async Task<bool> IdExistsAsync(Guid id)
        {
            return await context.Users.AnyAsync(x => x.Id == id);
        }

        /// <inheritdoc/>
        public async Task<User> AddAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        /// <inheritdoc/>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        /// <inheritdoc/>
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}