using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    public class AuditLogRepository(InventoryManagementContext context) : IAuditLogRepository
    {
        public async Task<List<AuditLog>> GetAllAuditLogsAsync()
        {
            return await context.AuditLog.ToListAsync();
        }

        public async Task<List<AuditLog>> GetAllForDateAsync(DateTime date)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await context.AuditLog
                .Where(x => x.Timestamp >= start && x.Timestamp < end)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetAllForUserAsync(Guid userId)
        {

            return await context.AuditLog.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task AddAsync(AuditLog log)
        {
            await context.AddRangeAsync(log);
            await context.SaveChangesAsync();
        }
    }
}
