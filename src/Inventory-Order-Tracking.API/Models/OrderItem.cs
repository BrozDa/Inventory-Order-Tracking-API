namespace Inventory_Order_Tracking.API.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } //nav prop

        public Guid ProductId { get; set; }
        public Product Product { get; set; } //nav prop
        public int OrderedQuantity { get; set; }
    }
}
