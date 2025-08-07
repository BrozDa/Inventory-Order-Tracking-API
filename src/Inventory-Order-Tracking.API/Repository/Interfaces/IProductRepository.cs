using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    /// <summary>
    /// Defines a contract for retrieving and managing products in data storage.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Adds a new <see cref="Product"/> to the data storage
        /// </summary>
        /// <param name="entity">A <see cref="Product"/> to be added</param>
        /// <returns>A newly added <see cref="Product"/> with auto-generated Id</returns>
        Task<Product> AddAsync(Product entity);

        /// <summary>
        /// Removes single <see cref="Product"/> from data storage
        /// </summary>
        /// <param name="entity">A <see cref="Product"/> to be removed</param>
        Task DeleteAsync(Product entity);

        /// <summary>
        /// Retrieves all products from the data storage.
        /// </summary>
        /// <returns>A list of all <see cref="Product"/> entries.</returns>
        Task<List<Product>> GetAllAsync();

        /// <summary>
        /// Retrieves single <see cref="Product"/> from data storage based on provided id.
        /// </summary>
        /// <param name="productId">An Id of a <see cref="Product"/> to be retrieved</param>
        /// <returns>A retrieved <see cref="Product"/> if its found, null otherwise</returns>
        Task<Product?> GetByIdAsync(Guid productId);

        /// <summary>
        /// Persists all pending changes made to entities in data storage.
        /// </summary>
        Task SaveChangesAsync();
    }
}