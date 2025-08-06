using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Domain
{
    /// <summary>
    /// Represents available stock values for <see cref="ProductCustomerDto"/>
    /// </summary>
    public enum StockStatus
    {
        Unavailable,
        Low,
        Available
    }
}