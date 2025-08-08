using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Serilog;

namespace Inventory_Order_Tracking.API.Services
{
    //NOTE: Results are fire and forget -> not refactoring to void Tasks for now as the result might be handy in future

    /// <summary>
    /// Defines operations for retrieving and managing audit logs within the app.
    /// </summary>
    public class AuditLogService(
        IAuditLogRepository auditLogRepo,
        IUserRepository userRepository,
        ILogger<AuditLogService> logger
        ) : IAuditLogService
    {
        /// <inheritdoc/>
        public async Task<ServiceResult<List<AuditLogDto>>> GetAllAsync()
        {
            try
            {
                var logs = await auditLogRepo.GetAllAuditLogsAsync();

                return ServiceResult<List<AuditLogDto>>.Success(
                    data: logs.Select(x => x.ToDto()).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllAsync] Unhandled Exception has occured");

                return ServiceResult<List<AuditLogDto>>.Failure(
                    errors: ["Failed to fetch logs"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForDateAsync(DateTime date)
        {
            try
            {
                if (date > DateTime.UtcNow)
                {
                    logger.LogWarning("[AuditService][GetAllForDateAsync] Attempt to gather logs for future date");

                    return ServiceResult<List<AuditLogDto>>.Failure(
                        errors: ["Cannot get logs for future date"],
                        statusCode: 400);
                }
                var logs = await auditLogRepo.GetAllForDateAsync(date);

                return ServiceResult<List<AuditLogDto>>.Success(
                    data: logs.Select(x => x.ToDto()).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllForDateAsync] Unhandled Exception has occured");
                return ServiceResult<List<AuditLogDto>>.Failure(
                    errors: ["Failed to fetch logs"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<AuditLogDto>>> GetAllForUserAsync(Guid userId)
        {
            try
            {
                if (!await userRepository.IdExistsAsync(userId))
                {
                    logger.LogWarning("[AuditService][GetAllForUserAsync] Attempt to gather logs for non existing user");

                    return ServiceResult<List<AuditLogDto>>.Failure(
                        errors: ["User with provided Id does not exist"],
                        statusCode: 400);
                }

                var logs = await auditLogRepo.GetAllForUserAsync(userId);

                return ServiceResult<List<AuditLogDto>>.Success(
                    data: logs.Select(x => x.ToDto()).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][GetAllForUserAsync] Unhandled Exception has occured");
                return ServiceResult<List<AuditLogDto>>.Failure(
                    errors: ["Failed to fetch logs"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<AuditLogDto>> AddNewLogAsync(AuditLogAddDto log)
        {
            try
            {
                if (!await userRepository.IdExistsAsync(log.UserId))
                {
                    logger.LogWarning("[AuditService][AddNewLogAsync] Attempt to add log for non existent user");

                    return ServiceResult<AuditLogDto>.Failure(
                        errors: ["Cannot add logs for non existent user"],
                        statusCode: 400);
                }

                var newLog = new AuditLog
                {
                    UserId = log.UserId,
                    Action = log.Action,
                };

                await auditLogRepo.AddAsync(newLog);

                return ServiceResult<AuditLogDto>.Success(
                    data: newLog.ToDto(),
                    statusCode: 201);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[AuditService][AddNewLogAsync] Unhandled Exception has occured");
                return ServiceResult<AuditLogDto>.Failure(
                    errors: ["Failed to add new log"],
                    statusCode: 500);
            }
        }
    }
}