using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> GetAllAuditLogsAsync();
        Task<List<AuditLog>> GetAllForUserAsync(Guid userId);
        Task<List<AuditLog>> GetAllForDateAsync(DateTime date);
        Task AddAsync(AuditLog log);
    }
}
