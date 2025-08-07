using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    /// <summary>
    /// Provides data access and persistence operations for <see cref="Order"/> entities.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IOrderRepository"/> interface to interact with the database
    /// using Entity Framework Core.
    /// </remarks>
    public class OrderRepository(InventoryManagementContext context) : IOrderRepository
    {
        /// <inheritdoc/>
        public async Task<Order> CreateNewOrderAsync(Guid userId)
        {
            var newOrder = new Order { UserId = userId, Status =  OrderStatus.Submitted};

            await context.Orders.AddAsync(newOrder);

            return newOrder;
        }
        /// <inheritdoc/>
        public async Task<List<OrderItem>> AddOrderItemsAsync(List<OrderItem> items)
        {
            await context.AddRangeAsync(items);

            return items;
        }
        /// <inheritdoc/>
        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        }
        /// <inheritdoc/>
        public async Task<List<Order>> GetAllForUserAsync(Guid userId)
        {
            return await context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
