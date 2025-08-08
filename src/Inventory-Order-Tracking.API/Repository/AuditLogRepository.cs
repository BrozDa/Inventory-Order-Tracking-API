using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    /// <summary>
    /// Provides data access and persistence operations for <see cref="Product"/> entities.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IProductRepository"/> interface to interact with the database
    /// using Entity Framework Core.
    /// </remarks>
    public class AuditLogRepository(InventoryManagementContext context) : IAuditLogRepository
    {
        /// <inheritdoc/>
        public async Task<List<AuditLog>> GetAllAuditLogsAsync()
        {
            return await context.AuditLog.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<AuditLog>> GetAllForDateAsync(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await context.AuditLog
                .Where(x => x.Timestamp >= start && x.Timestamp < end)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<AuditLog>> GetAllForUserAsync(Guid userId)
        {
            return await context.AuditLog.Where(x => x.UserId == userId).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task AddAsync(AuditLog log)
        {
            await context.AddRangeAsync(log);
            await context.SaveChangesAsync();
        }
    }
}