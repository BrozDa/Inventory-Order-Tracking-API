using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);

        Task<bool> IdExists(Guid id);
        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByIdAsync(Guid id);

        Task<User> AddAsync(User user);

        Task SaveChangesAsync();
    }
}