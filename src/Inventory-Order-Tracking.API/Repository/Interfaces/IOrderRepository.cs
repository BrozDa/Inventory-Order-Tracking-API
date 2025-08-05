using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateNewOrderAsync(Guid userId);
        Task<List<OrderItem>> AddOrderItemsAsync(List<OrderItem> items);
        Task SaveChangesAsync();
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<List<Order>> GetAllForUserAsync(Guid userId);
    }
}
