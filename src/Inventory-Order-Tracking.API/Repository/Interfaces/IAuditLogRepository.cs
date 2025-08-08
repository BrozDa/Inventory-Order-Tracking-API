using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    /// <summary>
    /// Defines a contract for accessing and managing audit logs in data storage.
    /// </summary>
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Fetches all audit logs from the data storage.
        /// </summary>
        /// <returns>A list of all <see cref="AuditLog"/> entries.</returns>
        Task<List<AuditLog>> GetAllAuditLogsAsync();

        /// <summary>
        /// Fetches all audit logs associated with provided user id from the data storage.
        /// </summary>
        /// <param name="userId">An <see cref="Guid"/> of user for which logs will be retrieved</param>
        /// <returns>A list of filtered <see cref="AuditLog"/> entries.</returns>
        Task<List<AuditLog>> GetAllForUserAsync(Guid userId);

        /// <summary>
        /// Fetches all audit logs associated created at provided date from the data storage.
        /// </summary>
        /// <param name="date">A <see cref="DateTime"/> for which logs will be retrieved; Only Date portion is used for the filter</param>
        /// <returns>A list of filtered <see cref="AuditLog"/> entries.</returns>
        Task<List<AuditLog>> GetAllForDateAsync(DateTime date);

        /// <summary>
        /// Adds a new log to the data storage
        /// </summary>
        /// <param name="log">A <see cref="AuditLog"/> to be added </param>
        Task AddAsync(AuditLog log);
    }
}