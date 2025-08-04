using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IAuditService
    {
        Task<ServiceResult<List<AuditLogDto>>> GetAllAsync();
        Task<ServiceResult<List<AuditLogDto>>> GetAllForUser(Guid userId);
        Task<ServiceResult<List<AuditLogDto>>> GetAllForDate(DateTime date);
    }
}
