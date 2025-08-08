namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object for adding a new audit log.
    /// </summary>
    public class AuditLogAddDto
    {
        public Guid UserId { get; set; }
        public string Action { get; set; } = string.Empty;
    }
}