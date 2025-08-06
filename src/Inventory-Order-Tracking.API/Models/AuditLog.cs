using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Models
{
    /// <summary>
    /// Represents an audit log entry used to persist user actions and activity
    /// </summary>
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Maps an existing <see cref="AuditLog"/> to <see cref="AuditLogDto"/>
        /// </summary>
        /// <returns>An instance <see cref="AuditLogDto"/> with mapped values</returns>
        public AuditLogDto ToDto()
        {
            return new AuditLogDto
            {
                Id = Id,
                UserId = UserId,
                Timestamp = Timestamp,
                Action = Action,
            };
        }
    }
}