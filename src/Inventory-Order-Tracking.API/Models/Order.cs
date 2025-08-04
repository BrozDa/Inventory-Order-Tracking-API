using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } //nav prop

        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal OrderPrice { get; set; } = 0;
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public OrderDto ToDto()
        {
            return new OrderDto()
            {
                Id = Id,
                UserId = UserId,
                Status = Status,
                OrderDate = OrderDate,
                OrderPrice = OrderPrice,
                Items = Items.Select(x => x.ToDto()).ToList()
            };
        }
    }
}