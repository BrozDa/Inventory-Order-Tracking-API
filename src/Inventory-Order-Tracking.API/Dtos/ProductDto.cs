using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public StockStatus StockStatus { get; set; } = new();

        public decimal Price { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }


}
