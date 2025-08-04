namespace Inventory_Order_Tracking.API.Dtos
{
    public class CreateOrderDto
    {
       public List<OrderItemDto> Items { get; set; } = new();
    }
}
