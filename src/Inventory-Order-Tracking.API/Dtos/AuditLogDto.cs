using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Dtos
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Action { get; set; } = string.Empty;
    }
}

