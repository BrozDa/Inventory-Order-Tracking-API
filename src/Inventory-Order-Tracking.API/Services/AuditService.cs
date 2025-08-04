using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    public class AuditService(
        IAuditLogRepository auditLogRepo,
        IUserRepository userRepository,
        ILogger<AuditService> logger
        ) : IAuditService
    {
        public async Task<ServiceResult<List<AuditLogDto>>> GetAllAsync()
        {
            try
            {
                var logs = await auditLogRepo.GetAllAuditLogsAsync();

                return ServiceResult<List<AuditLogDto>>.Ok(logs.Select(x => x.ToDto()).ToList());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllAsync] Unhandled Exception has occured");
                return ServiceResult<List<AuditLogDto>>.InternalServerError("Failed to fetch logs");
            }
        }

        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForDate(DateTime date)
        {
            try
            {
                if (date > DateTime.Now) 
                {
                    logger.LogWarning("[AuditService][GetAllForDate] Attempt to gather logs for future date");
                    return ServiceResult<List<AuditLogDto>>.BadRequest("Cannot get logs for future date");
                }
                var logs = await auditLogRepo.GetAllForDateAsync(date);

                return ServiceResult<List<AuditLogDto>>.Ok(logs.Select(x => x.ToDto()).ToList());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllForDate] Unhandled Exception has occured");
                return ServiceResult<List<AuditLogDto>>.InternalServerError("Failed to fetch logs");
            }
        }

        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForUser(Guid userId)
        {
            try
            {
                if(! await userRepository.IdExists(userId))
                {
                    logger.LogWarning("[AuditService][GetAllForUser] Attempt to gather logs for non existing user");
                    return ServiceResult<List<AuditLogDto>>.BadRequest("User with provided Id does not exist");
                }

                var logs = await auditLogRepo.GetAllForUserAsync(userId);

                return ServiceResult<List<AuditLogDto>>.Ok(logs.Select(x => x.ToDto()).ToList());

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllForUser] Unhandled Exception has occured");
                return ServiceResult<List<AuditLogDto>>.InternalServerError("Failed to fetch logs");
            }
        }
    }
}
