namespace Inventory_Order_Tracking.API.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } //nav prop

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}