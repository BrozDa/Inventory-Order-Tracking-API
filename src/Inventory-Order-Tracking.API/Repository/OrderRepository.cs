using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    public class OrderRepository(InventoryManagementContext context) : IOrderRepository
    {

        public async Task<Order> CreateNewOrder(Guid userId)
        {
            var newOrder = new Order { UserId = userId, Status =  OrderStatus.Submitted};

            await context.Orders.AddAsync(newOrder);

            return newOrder;
        }
        public async Task<List<OrderItem>> AddOrderItems(List<OrderItem> items)
        {
            await context.AddRangeAsync(items);

            return items;
        }
        public async Task<Order?> GetById(Guid orderId)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
