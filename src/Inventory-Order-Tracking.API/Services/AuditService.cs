using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{

    //NOTE: Results are fire and forget -> not refactoring to void Tasks for now as the result might be handy in future
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

        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForDateAsync(DateTime date)
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

        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForUserAsync(Guid userId)
        {
            try
            {
                if(! await userRepository.IdExistsAsync(userId))
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

        public async Task<ServiceResult<AuditLogDto>> AddNewLogAsync(AuditLogAddDto log)
        {
            try
            {
                if (!await userRepository.IdExistsAsync(log.UserId))
                {
                    logger.LogWarning("[AuditService][AddNewLogAsync] Attempt to add log for non existent user");
                    return ServiceResult<AuditLogDto>.BadRequest("Cannot add logs for non existent user");
                }

                var newLog = new AuditLog
                {
                    UserId = log.UserId,
                    Action = log.Action,
                };

                await auditLogRepo.AddAsync(newLog);


                return ServiceResult<AuditLogDto>.Created(newLog.ToDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][AddNewLogAsync] Unhandled Exception has occured");
                return ServiceResult<AuditLogDto>.InternalServerError("Failed to add new log");
            }

        }
    }
}
