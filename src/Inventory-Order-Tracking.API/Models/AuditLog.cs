using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Models
{
    public class AuditLog
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } // nav prop

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty;

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