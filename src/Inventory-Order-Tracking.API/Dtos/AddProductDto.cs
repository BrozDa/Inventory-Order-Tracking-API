using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Dtos
{
    public class AddProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }

    }
}
