using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateNewOrder(Guid userId);
        Task<List<OrderItem>> AddOrderItems(List<OrderItem> items);
        Task SaveChangesAsync();
    }
}
