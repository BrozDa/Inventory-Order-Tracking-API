using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    /// <summary>
    /// Defines a contract for retrieving and managing users in data storage.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Checks if there is an user with provided username present in the data storage
        /// </summary>
        /// <param name="username">A <see cref="string"/> representation of the username</param>
        /// <returns>A true if there is user with matching username, false otherwise </returns>
        Task<bool> UsernameExistsAsync(string username);

        /// <summary>
        /// Checks if there is an user with provided id present in the data storage
        /// </summary>
        /// <param name="id">A <see cref="Guid"/> representation of the user Id</param>
        /// <returns>A true if there is user with matching id, false otherwise </returns>
        Task<bool> IdExistsAsync(Guid id);

        /// <summary>
        /// Retrieves a <see cref="User"/> from data storage based on its username
        /// </summary>
        /// <param name="username">A <see cref="string"/> representation of the username</param>
        /// <returns>A <see cref="User"/> if there is user with matching name, null otherwise </returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Retrieves a <see cref="User"/> from data storage based on its id
        /// </summary>
        /// <param name="id">A <see cref="Guid"/> representation of the user id</param>
        /// <returns>A <see cref="User"/> if there is user with matching id, null otherwise </returns>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds a new <see cref="User"/> to the data storage
        /// </summary>
        /// <param name="user">A <see cref="User"/> to be added</param>
        /// <returns>A newly added <see cref="User"/> with auto-generated Id</returns>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Persists all pending changes made to entities in data storage.
        /// </summary>
        Task SaveChangesAsync();
    }
}