using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Models
{
    /// <summary>
    /// Represents a item contained in the order.
    /// </summary>
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int OrderedQuantity { get; set; }
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Maps an existing <see cref="OrderItem"/> to <see cref="OrderItemDto"/>
        /// </summary>
        /// <returns>An instance <see cref="OrderItemDto"/> with mapped values</returns>
        public OrderItemDto ToDto()
        {
            return new OrderItemDto
            {
                Id = Id,
                ProductId = ProductId,
                ProductName = Product.Name,
                Quantity = OrderedQuantity,
                UnitPrice = UnitPrice,
            };
        }
    }
}