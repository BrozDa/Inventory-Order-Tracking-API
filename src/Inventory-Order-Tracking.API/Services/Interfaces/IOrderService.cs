using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations for retrieving and managing orders within the app.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Verifies and adds new order to the data storage
        /// </summary>
        /// <param name="userId">An Id of <see cref="User"/> submitting the order</param>
        /// <param name="dto">A <see cref="OrderCreateDto"/> containing list of ordered items</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing <see cref="OrderDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<OrderDto>> SubmitOrderAsync(Guid userId, OrderCreateDto dto);

        /// <summary>
        /// Retrieves a single order from data storage
        /// </summary>
        /// <param name="userId">An Id of <see cref="User"/> associated with the order</param>
        /// <param name="orderId">An Id of the order to be retrieved</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing a list of <see cref="OrderDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<OrderDto>> GetOrderByIdAsync(Guid userId, Guid orderId);

        /// <summary>
        /// Retrieves all orders associated with user from data storage
        /// </summary>
        /// <param name="userId">An Id of <see cref="User"/> for whom orders will be retrieved</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing <see cref="OrderDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<OrderDto>>> GetAllOrdersForUserAsync(Guid userId);

        /// <summary>
        /// Cancels a order (changing its state in the data storage)
        /// </summary>
        /// <param name="userId">An Id of <see cref="User"/> associated with the order</param>
        /// <param name="orderId">An Id of the order to be cancelled</param>
        /// <returns> An <see cref="ServiceResult{T}"/> containing the cancelled <see cref="OrderDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<OrderDto>> CancelOrderAsync(Guid userId, Guid orderId);
    }
}