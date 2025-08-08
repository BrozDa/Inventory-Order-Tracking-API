namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object for creating new order.
    /// </summary>
    public class OrderCreateDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
    }
}