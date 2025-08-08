using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations for retrieving and managing audit logs within the app.
    /// </summary>
    public interface IAuditLogService
    {
        /// <summary>
        /// Retrieves all audit logs from the data storage.
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing a list of <see cref="AuditLogDto"/> objects on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<AuditLogDto>>> GetAllAsync();

        /// <summary>
        /// Retrieves all audit logs associated with specific user from from the data storage.
        /// </summary>
        /// <param name="userId">The Id of the user whose logs are to be retrieved.</param>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing a list of <see cref="AuditLogDto"/> objects on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<AuditLogDto>>> GetAllForUserAsync(Guid userId);

        /// <summary>
        /// Retrieves all audit logs created on specific date.
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing a list of <see cref="AuditLogDto"/> objects on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<AuditLogDto>>> GetAllForDateAsync(DateTime date);

        /// <summary>
        /// Adds new audit log to the data storage
        /// </summary>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> newly added log in form of  <see cref="AuditLogDto"/> object on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<AuditLogDto>> AddNewLogAsync(AuditLogAddDto log);
    }
}