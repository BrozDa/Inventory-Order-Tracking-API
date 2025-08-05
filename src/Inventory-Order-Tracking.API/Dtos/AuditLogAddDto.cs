namespace Inventory_Order_Tracking.API.Dtos
{
    public class AuditLogAddDto
    {
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}
