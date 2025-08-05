using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    public class UserRepository(InventoryManagementContext context) : IUserRepository
    {
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IdExistsAsync(Guid id)
        {
            return await context.Users.AnyAsync(x => x.Id == id);
        }
        public async Task<User> AddAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}