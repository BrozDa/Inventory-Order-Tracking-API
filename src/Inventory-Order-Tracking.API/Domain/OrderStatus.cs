namespace Inventory_Order_Tracking.API.Domain
{
    /// <summary>
    /// Represents available status for the order
    /// </summary>
    public enum OrderStatus
    {
        Submitted,
        InProgress,
        Completed,
        Cancelled
    }
}