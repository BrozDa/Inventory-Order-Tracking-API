using Inventory_Order_Tracking.API.Domain;

namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object for fetching existing order.
    /// </summary>
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderPrice { get; set; } = 0;
        public ICollection<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}