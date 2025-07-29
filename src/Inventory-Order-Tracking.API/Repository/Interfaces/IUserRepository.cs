using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<User?> GetByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}
