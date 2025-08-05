using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<ServiceResult<List<AuditLogDto>>> GetAllAsync();
        Task<ServiceResult<List<AuditLogDto>>> GetAllForUserAsync(Guid userId);
        Task<ServiceResult<List<AuditLogDto>>> GetAllForDateAsync(DateTime date);
        
        Task<ServiceResult<AuditLogDto>> AddNewLogAsync(AuditLogAddDto log);
    }
}
