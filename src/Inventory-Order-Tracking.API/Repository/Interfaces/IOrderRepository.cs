using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    /// <summary>
    /// Defines a contract for accessing and managing orders in data storage.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Adds new <see cref="Order"/> to data storage
        /// </summary>
        /// <param name="userId">An Id of the <see cref="User"/> associated with the new order</param>
        /// <returns>A newly added <see cref="Order"/> with auto-generated Id</returns>
        Task<Order> CreateNewOrderAsync(Guid userId);

        /// <summary>
        /// Adds list of <see cref="OrderItem"/> to the data storage
        /// </summary>
        /// <param name="items">List of items to be added</param>
        /// <returns>Return list with added items with auto-generated Ids</returns>
        Task<List<OrderItem>> AddOrderItemsAsync(List<OrderItem> items);
        /// <summary>
        /// Retrieves single <see cref="Order"/> from data storage based on provided id.
        /// </summary>
        /// <param name="orderId">An Id of a <see cref="Order"/> to be retrieved</param>
        /// <returns>A retrieved <see cref="Order"/> if its found, null otherwise</returns>
        Task<Order?> GetByIdAsync(Guid orderId);

        /// <summary>
        /// Retrieves all orders associated with provided user id from the data storage.
        /// </summary>
        /// <param name="userId">An <see cref="Guid"/> of user for which orders will be retrieved</param>
        /// <returns>A list of filtered <see cref="AuditLog"/> entries.</returns>
        Task<List<Order>> GetAllForUserAsync(Guid userId);

        /// <summary>
        /// Persists all pending changes made to entities in data storage.
        /// </summary>
        Task SaveChangesAsync();
    }
}
